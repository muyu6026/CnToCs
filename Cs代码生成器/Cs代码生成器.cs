// 引入System.Text命名空间，用于StringBuilder字符串拼接
using System.Text;
// 引入LINQ命名空间，支持Select等扩展方法
using System.Linq;
// 引入IO命名空间，用于文件路径操作
using System.IO;
// 引入泛型集合命名空间
using System.Collections.Generic;
// 引入AST命名空间，访问抽象语法树相关节点
using CnToCs.抽象语法树;

// 定义代码生成器相关的命名空间
namespace CnToCs.Cs代码生成器
{
    /// <summary>
    /// C#代码生成器，用于将AST节点生成C#代码
    /// </summary>
    public class Cs代码生成器
    {
        /// <summary>
        /// 生成C#代码的主方法，根据不同AST节点类型生成对应代码
        /// </summary>
        /// <param name="节点">AST节点</param>
        /// <returns>生成的C#代码字符串</returns>
        public string 生成代码(语法树节点 节点)
        {
            return 生成代码(节点, null);
        }

        /// <summary>
        /// 生成C#代码的主方法，根据不同AST节点类型生成对应代码
        /// </summary>
        /// <param name="节点">AST节点</param>
        /// <param name="输入文件路径">输入文件路径，用于生成唯一命名空间</param>
        /// <returns>生成的C#代码字符串</returns>
        public string 生成代码(语法树节点 节点, string 输入文件路径)
        {
            // 用于拼接生成的C#代码
            var 代码构建器 = new StringBuilder();
            
            // 如果节点是根节点，递归生成所有子节点的代码
            if (节点 is 根节点 根节点)
            {
                // 添加文件头部注释
                代码构建器.AppendLine("// 此文件由中文转C#编译器自动生成");
                代码构建器.AppendLine("// 请勿手动修改此文件");
                代码构建器.AppendLine();
                
                // 生成基于文件名的命名空间
                string 命名空间 = "GeneratedCode";
                if (!string.IsNullOrEmpty(输入文件路径))
                {
                    // 获取不带扩展名的文件名，并清理特殊字符
                    string 文件名 = Path.GetFileNameWithoutExtension(输入文件路径);
                    // 清理文件名中的特殊字符，只保留字母、数字和中文
                    var 清理后的文件名 = new StringBuilder();
                    foreach (char c in 文件名)
                    {
                        if (char.IsLetterOrDigit(c) || c >= 0x4E00 && c <= 0x9FFF) // 中文字符范围
                        {
                            清理后的文件名.Append(c);
                        }
                    }
                    if (清理后的文件名.Length > 0)
                    {
                        命名空间 = $"Generated_{清理后的文件名}";
                    }
                }
                
                // 添加必要的using语句（在命名空间外部）
                代码构建器.AppendLine("using System;");
                代码构建器.AppendLine("using System.Collections.Generic;");
                代码构建器.AppendLine();
                
                // 添加命名空间声明
                代码构建器.AppendLine($"namespace {命名空间}");
                代码构建器.AppendLine("{");
                代码构建器.AppendLine();
                
                // 遍历根节点下的所有子节点
                foreach (var 子节点 in 根节点.子节点列表)
                {
                    // 递归生成子节点代码并换行
                    string 子节点代码 = 生成代码(子节点, 输入文件路径);
                    // 为子节点的每一行添加缩进
                    var 缩进后的代码 = new StringBuilder();
                    foreach (var 行 in 子节点代码.Split('\n'))
                    {
                        if (!string.IsNullOrWhiteSpace(行))
                        {
                            缩进后的代码.AppendLine("    " + 行);
                        }
                        else
                        {
                            缩进后的代码.AppendLine();
                        }
                    }
                    代码构建器.Append(缩进后的代码.ToString());
                    // 每个节点之间额外空一行
                    代码构建器.AppendLine();
                }
                
                // 结束命名空间声明
                代码构建器.AppendLine("}");
            }
            // 如果节点是接口节点，生成C#接口代码
            else if (节点 is 接口节点 接口节点)
            {
                // 输出接口声明行，如果有基接口则添加继承关系
                if (接口节点.基接口列表 != null && 接口节点.基接口列表.Count > 0)
                {
                    代码构建器.AppendLine($"public interface {接口节点.名称} : {string.Join(", ", 接口节点.基接口列表)}");
                }
                else
                {
                    代码构建器.AppendLine($"public interface {接口节点.名称}");
                }
                代码构建器.AppendLine("{");
                
                // 遍历接口的所有属性，生成属性声明
                foreach (var 属性 in 接口节点.属性列表)
                {
                    // 输出属性名、可空标记和类型
                    var 属性声明 = $"    {属性.修饰符} {属性.类型.类型名称}{(属性.是否可为空 ? "?" : "")} {属性.名称} {{ get; set; }}";
                    
                    // 如果有初始化值，添加默认值
                    if (属性.初始化值 != null)
                    {
                        var 初始化值代码 = 生成表达式代码(属性.初始化值.表达式);
                        属性声明 = 属性声明.Replace(" { get; set; }", $" {{ get; set; }} = {初始化值代码}");
                    }
                    
                    代码构建器.AppendLine(属性声明);
                }
                
                // 如果有方法，生成方法声明
                if (接口节点.方法列表.Count > 0)
                {
                    代码构建器.AppendLine();
                    foreach (var 方法 in 接口节点.方法列表)
                    {
                        // 生成方法参数列表
                        var 参数列表 = string.Join(", ", 方法.参数列表.Select(p => $"{p.参数类型.类型名称}{(p.是否可为空 ? "?" : "")} {p.参数名称}"));
                        
                        // 生成方法声明，检查是否为虚方法
                        if (方法.是否为虚方法)
                        {
                            代码构建器.AppendLine($"    virtual {方法.返回类型} {方法.名称}({参数列表});");
                        }
                        else
                        {
                            代码构建器.AppendLine($"    {方法.返回类型} {方法.名称}({参数列表});");
                        }
                    }
                }
                
                // 结束接口声明
                代码构建器.AppendLine("}");
            }
            // 如果节点是类节点，生成C#类代码
            else if (节点 is 类节点 类节点)
            {
                // 输出类声明行，如果有基类则添加继承关系
                var 继承列表 = new List<string>();
                if (!string.IsNullOrEmpty(类节点.基类名称))
                {
                    继承列表.Add(类节点.基类名称);
                }
                if (类节点.基接口列表 != null && 类节点.基接口列表.Count > 0)
                {
                    继承列表.AddRange(类节点.基接口列表);
                }
                
                if (继承列表.Count > 0)
                {
                    代码构建器.AppendLine($"public class {类节点.名称} : {string.Join(", ", 继承列表)}");
                }
                else
                {
                    代码构建器.AppendLine($"public class {类节点.名称}");
                }
                代码构建器.AppendLine("{");
                
                // 遍历类的所有属性，生成属性声明
                foreach (var 属性 in 类节点.属性列表)
                {
                    // 输出属性名、可空标记和类型
                    var 修饰符 = 属性.修饰符;
                    // readonly不能用于属性，只能用于字段
                    if (修饰符 == "readonly")
                    {
                        修饰符 = ""; // 移除readonly修饰符
                    }
                    var 属性声明 = $"    {修饰符} {属性.类型.类型名称}{(属性.是否可为空 ? "?" : "")} {属性.名称} {{ get; set; }}";
                    
                    // 如果有初始化值，添加默认值
                    if (属性.初始化值 != null)
                    {
                        var 初始化值代码 = 生成表达式代码(属性.初始化值.表达式);
                        属性声明 = 属性声明.Replace(" { get; set; }", $" {{ get; set; }} = {初始化值代码}");
                    }
                    
                    代码构建器.AppendLine(属性声明);
                }

                // 如果有构造函数，生成构造函数
                if (类节点.构造函数 != null)
                {
                    代码构建器.AppendLine();
                    代码构建器.Append("    public ");
                    代码构建器.Append(类节点.名称);
                    代码构建器.Append("(");
                    代码构建器.Append(string.Join(", ", 类节点.构造函数.参数列表.Select(p => $"{p.参数类型.类型名称} {p.参数名称}")));
                    代码构建器.AppendLine(")");
                    
                    // 只有当基类不是接口时才生成基类构造函数调用
                    if (!string.IsNullOrEmpty(类节点.基类名称))
                    {
                        // 检查基类是否在基接口列表中（即基类是否为接口）
                        bool 基类是接口 = 类节点.基接口列表.Contains(类节点.基类名称);
                        
                        if (!基类是接口)
                        {
                            代码构建器.Append(" : base(");
                            
                            // 对于基类构造函数，我们只传递第一个参数（通常是名称）
                            // 这是一个简化的实现，实际应该根据基类构造函数的签名来匹配参数
                            if (类节点.构造函数.参数列表.Count > 0)
                            {
                                // 只传递第一个参数给基类构造函数
                                代码构建器.Append(类节点.构造函数.参数列表[0].参数名称);
                            }
                            
                            代码构建器.AppendLine(")");
                        }
                        else
                        {
                            代码构建器.AppendLine();
                        }
                    }
                    else
                    {
                        代码构建器.AppendLine();
                    }
                    
                    代码构建器.AppendLine("    {");
                    
                    // 如果有方法体，生成方法体代码
                    if (类节点.构造函数.方法体 != null)
                    {
                        foreach (var 语句 in 类节点.构造函数.方法体.语句列表)
                        {
                            var 语句代码 = 生成语句代码(语句);
                            if (!string.IsNullOrEmpty(语句代码))
                            {
                                代码构建器.AppendLine($"        {语句代码}");
                            }
                        }
                    }
                    else
                    {
                        // 为每个参数赋值给对应的属性
                        foreach (var 参数 in 类节点.构造函数.参数列表)
                        {
                            // 检查类是否有对应的属性
                            var 对应属性 = 类节点.属性列表.FirstOrDefault(p => p.名称 == 参数.参数名称);
                            if (对应属性 != null)
                            {
                                代码构建器.AppendLine($"        this.{对应属性.名称} = {参数.参数名称};");
                            }
                            // 注意：基类的属性不再在这里赋值，而是通过基类构造函数传递
                        }
                    }
                    
                    代码构建器.AppendLine("    }");
                }
                
                // 如果有方法，生成方法
                if (类节点.方法列表.Count > 0)
                {
                    代码构建器.AppendLine();
                    foreach (var 方法 in 类节点.方法列表)
                    {
                        // 生成方法参数列表
                        var 参数列表 = string.Join(", ", 方法.参数列表.Select(p => $"{p.参数类型.类型名称}{(p.是否可为空 ? "?" : "")} {p.参数名称}"));
                        
                        // 生成方法声明
                        代码构建器.Append("    public ");
                        
                        // 处理不同的方法修饰符
                        if (方法.是否为抽象方法)
                        {
                            代码构建器.Append("abstract ");
                        }
                        else if (方法.是否为重写方法)
                        {
                            // 如果类实现了接口，则不需要override关键字
                            if (类节点.基接口列表 != null && 类节点.基接口列表.Count > 0)
                            {
                                // 实现接口方法，不需要override
                            }
                            else if (!string.IsNullOrEmpty(类节点.基类名称))
                            {
                                代码构建器.Append("override ");
                            }
                            else
                            {
                                // 如果没有基类，也不需要override
                            }
                        }
                        else if (!string.IsNullOrEmpty(类节点.基类名称) && 方法.是否为虚方法)
                        {
                            代码构建器.Append("override ");
                        }
                        else if (!string.IsNullOrEmpty(类节点.基类名称))
                        {
                            // 如果有基类但方法不是虚方法，添加new关键字避免隐藏警告
                            代码构建器.Append("new ");
                        }
                        else if (方法.是否为虚方法)
                        {
                            代码构建器.Append("virtual ");
                        }
                        
                        代码构建器.Append($"{方法.返回类型} {方法.名称}({参数列表})");
                        
                        // 如果是抽象方法，直接结束声明，不添加方法体
                        if (方法.是否为抽象方法)
                        {
                            代码构建器.AppendLine(";");
                            continue;
                        }
                        
                        代码构建器.AppendLine();
                        代码构建器.AppendLine("    {");
                        
                        // 如果有方法体，生成方法体代码
                        if (方法.方法体 != null)
                        {
                            foreach (var 语句 in 方法.方法体.语句列表)
                            {
                                var 语句代码 = 生成语句代码(语句);
                                if (!string.IsNullOrEmpty(语句代码))
                                {
                                    代码构建器.AppendLine($"        {语句代码}");
                                }
                            }
                        }
                        else
                        {
                            // 如果返回类型不是void，添加默认返回语句
                            if (方法.返回类型 != "void")
                            {
                                if (方法.返回类型 == "string")
                                {
                                    代码构建器.AppendLine($"        return string.Empty;");
                                }
                                else if (方法.返回类型 == "int" || 方法.返回类型 == "double")
                                {
                                    代码构建器.AppendLine($"        return 0;");
                                }
                                else if (方法.返回类型 == "bool")
                                {
                                    代码构建器.AppendLine($"        return false;");
                                }
                                else
                                {
                                    代码构建器.AppendLine($"        return default({方法.返回类型});");
                                }
                            }
                        }
                        
                        代码构建器.AppendLine("    }");
                    }
                }
                
                // 结束类声明
                代码构建器.AppendLine("}");
            }
            
            // 返回最终生成的C#代码字符串
            return 代码构建器.ToString();
        }
        
        /// <summary>
        /// 生成表达式代码
        /// </summary>
        /// <param name="表达式">表达式节点</param>
        /// <returns>表达式代码字符串</returns>
        private string 生成表达式代码(表达式节点 表达式)
        {
            if (表达式 is 字面量表达式节点 字面量)
            {
                if (字面量.类型 == "string")
                {
                    return $"\"{字面量.值}\"";
                }
                else if (字面量.类型 == "null")
                {
                    return "null";
                }
                else
                {
                    return 字面量.值;
                }
            }
            else if (表达式 is 标识符表达式节点 标识符)
            {
                // 检查是否是基类方法调用
                if (标识符.名称.StartsWith("基础"))
                {
                    // 提取方法名，去掉"基础"前缀
                    var 方法名 = 标识符.名称.Substring(2);
                    return $"base.{方法名}";
                }
                return 标识符.名称;
            }
            else if (表达式 is 二元运算表达式节点 二元运算)
            {
                var 左操作数 = 生成表达式代码(二元运算.左操作数);
                var 右操作数 = 生成表达式代码(二元运算.右操作数);
                return $"({左操作数} {二元运算.运算符} {右操作数})";
            }
            else if (表达式 is 方法调用表达式节点 方法调用)
            {
                var 目标 = 生成表达式代码(方法调用.目标);
                var 参数列表 = string.Join(", ", 方法调用.参数列表.Select(p => 生成表达式代码(p)));
                return $"{目标}({参数列表})";
            }
            else if (表达式 is 属性访问表达式节点 属性访问)
            {
                var 目标 = 生成表达式代码(属性访问.目标);
                return $"{目标}.{属性访问.属性名}";
            }
            else
            {
                return "/* 不支持的表达式 */";
            }
        }
        
        /// <summary>
        /// 生成语句代码
        /// </summary>
        /// <param name="语句">语句节点</param>
        /// <returns>语句代码字符串</returns>
        private string 生成语句代码(语句节点 语句)
        {
            if (语句 is 返回语句节点 返回语句)
            {
                if (返回语句.表达式 != null)
                {
                    var 表达式代码 = 生成表达式代码(返回语句.表达式);
                    return $"return {表达式代码};";
                }
                else
                {
                    return "return;";
                }
            }
            else if (语句 is 赋值语句节点 赋值语句)
            {
                var 右侧代码 = 生成表达式代码(赋值语句.右侧);
                
                // 检查左侧是否是表达式节点
                if (赋值语句.左侧 is 表达式节点 左侧表达式)
                {
                    var 左侧代码 = 生成表达式代码(左侧表达式);
                    return $"{左侧代码} = {右侧代码};";
                }
                else
                {
                    return $"{赋值语句.左侧} = {右侧代码};";
                }
            }
            else if (语句 is 如果语句节点 如果语句)
            {
                return 生成如果语句代码(如果语句);
            }
            else
            {
                return "/* 不支持的语句 */";
            }
        }
        
        /// <summary>
        /// 生成如果语句代码
        /// </summary>
        /// <param name="如果语句">如果语句节点</param>
        /// <returns>如果语句代码字符串</returns>
        private string 生成如果语句代码(如果语句节点 如果语句)
        {
            var 条件代码 = 生成表达式代码(如果语句.条件);
            var 代码构建器 = new StringBuilder();
            
            代码构建器.Append($"if ({条件代码})\n");
            代码构建器.Append("{\n");
            
            // 生成then语句
            foreach (var 语句 in 如果语句.Then语句列表)
            {
                var 语句代码 = 生成语句代码(语句);
                if (!string.IsNullOrEmpty(语句代码))
                {
                    代码构建器.Append($"    {语句代码}\n");
                }
            }
            
            代码构建器.Append("}");
            
            // 生成else语句
            if (如果语句.Else语句列表 != null && 如果语句.Else语句列表.Count > 0)
            {
                代码构建器.Append("\nelse\n");
                代码构建器.Append("{\n");
                
                foreach (var 语句 in 如果语句.Else语句列表)
                {
                    var 语句代码 = 生成语句代码(语句);
                    if (!string.IsNullOrEmpty(语句代码))
                    {
                        代码构建器.Append($"    {语句代码}\n");
                    }
                }
                
                代码构建器.Append("}");
            }
            
            return 代码构建器.ToString();
        }
    }
}
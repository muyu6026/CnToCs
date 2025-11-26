// 引入System命名空间，提供基础功能
using System;
// 引入System.IO命名空间，用于文件操作
using System.IO;
// 引入词法分析器命名空间
using CnToCs.词法分析器;
// 引入语法分析器命名空间
using CnToCs.语法分析器;
// 引入代码生成器命名空间
using CnToCs.Cs代码生成器;

// 定义主程序相关的命名空间
namespace CnToCs
{
    /// <summary>
    /// 主程序类，负责协调整个编译流程
    /// </summary>
    class 程序
    {
        /// <summary>
        /// 程序入口点
        /// </summary>
        /// <param name="参数">命令行参数数组</param>
        static void Main(string[] 参数)
        {
            // 输出编译器启动信息
            Console.WriteLine("中文转C#编译器启动");
            Console.WriteLine($"参数数量: {参数.Length}");
            
            // 遍历并输出所有参数
            for (int i = 0; i < 参数.Length; i++)
            {
                Console.WriteLine($"参数 {i}: {参数[i]}");
            }
            
            // 检查参数数量，若无输入文件则提示并退出
            if (参数.Length < 1)
            {
                Console.WriteLine("请提供输入文件路径");
                Console.WriteLine("用法: 中文转C#编译器.exe <输入文件路径> [输出文件路径]");
                return;
            }

            // 获取输入文件路径
            string 输入文件路径 = 参数[0];
            // 获取输出文件路径，若未指定则自动更换扩展名为.cs
            string 输出文件路径 = 参数.Length > 1 ? 参数[1] : Path.ChangeExtension(输入文件路径, ".cs");

            // 输出输入输出文件路径
            Console.WriteLine($"输入文件: {输入文件路径}");
            Console.WriteLine($"输出文件: {输出文件路径}");

            try
            {
                // 检查输入文件是否存在
                if (!File.Exists(输入文件路径))
                {
                    Console.WriteLine($"输入文件不存在: {输入文件路径}");
                    Console.WriteLine("当前目录内容:");
                    // 输出当前目录下所有文件
                    foreach (var 文件 in Directory.GetFiles(Directory.GetCurrentDirectory()))
                    {
                        Console.WriteLine($"  {文件}");
                    }
                    return;
                }

                // 读取输入文件内容
                string 中文代码 = File.ReadAllText(输入文件路径);
                Console.WriteLine($"读取输入文件成功，内容长度: {中文代码.Length} 字符");
                
                // 编译中文代码为C#代码
                string CSharp代码 = 编译中文到Cs(中文代码, 输入文件路径);
                Console.WriteLine($"编译成功，生成 C# 代码长度: {CSharp代码.Length} 字符");
                
                // 确保输出目录存在
                string? 输出目录 = Path.GetDirectoryName(输出文件路径);
                if (!string.IsNullOrEmpty(输出目录) && !Directory.Exists(输出目录))
                {
                    Console.WriteLine($"创建输出目录: {输出目录}");
                    Directory.CreateDirectory(输出目录);
                }
                
                // 写入C#代码到输出文件
                File.WriteAllText(输出文件路径, CSharp代码);
                Console.WriteLine($"写入输出文件成功: {输出文件路径}");
                
                // 检查输出文件是否确实写入
                if (File.Exists(输出文件路径))
                {
                    Console.WriteLine($"输出文件确认存在，大小: {new FileInfo(输出文件路径).Length} 字节");
                }
                else
                {
                    Console.WriteLine("警告: 输出文件似乎未成功创建");
                }
            }
            // 捕获并输出异常信息
            catch (Exception 异常)
            {
                Console.WriteLine($"编译错误: {异常.Message}");
                Console.WriteLine($"堆栈跟踪: {异常.StackTrace}");
            }
        }

        /// <summary>
        /// 编译中文代码为C#代码的方法
        /// </summary>
        /// <param name="中文代码">输入的中文代码</param>
        /// <param name="输入文件路径">输入文件路径，用于生成唯一命名空间</param>
        /// <returns>生成的C#代码</returns>
        static string 编译中文到Cs(string 中文代码, string 输入文件路径)
        {
            // 执行完整的编译流程：词法分析 -> 语法分析 -> 代码生成
            try
            {
                Console.WriteLine("开始词法分析...");
                var 词法分析器实例 = new CnToCs.词法分析器.词法分析器(中文代码);
                Console.WriteLine("中文词法分析器 实例化完成");
                var 词法单元列表 = 词法分析器实例.分词();
                Console.WriteLine($"分词完成，词法单元数量: {词法单元列表.Count}");
                
                // 输出词法单元信息（用于调试）
                for (int i = 0; i < 词法单元列表.Count; i++)
                {
                    var 词法单元 = 词法单元列表[i];
                    Console.WriteLine($"词法单元[{i}]: 类型={词法单元.类型}, 值={词法单元.值}");
                }

                // 语法分析
                Console.WriteLine("开始语法分析...");
                var 语法分析器实例 = new CnToCs.语法分析器.语法分析器(词法单元列表);
                Console.WriteLine("中文语法分析器 实例化完成");
                var 抽象语法树 = 语法分析器实例.解析();
                Console.WriteLine($"语法分析完成，AST类型: {抽象语法树?.GetType().Name}");

                // 代码生成
                Console.WriteLine("开始代码生成...");
                var 代码生成器 = new CnToCs.Cs代码生成器.Cs代码生成器();
                Console.WriteLine("CSharp代码生成器 实例化完成");
                if (抽象语法树 == null)
                {
                    Console.WriteLine("抽象语法树为空，无法生成C#代码！");
                    return string.Empty;
                }
                
                // 获取语法分析器中的注释列表和节点注释映射
                var 注释列表 = 语法分析器实例.获取注释列表();
                var 节点注释映射 = 语法分析器实例.获取节点注释映射();
                var 属性注释映射 = 语法分析器实例.获取属性注释映射();
                var 方法注释映射 = 语法分析器实例.获取方法注释映射();
                
                Console.WriteLine($"注释列表数量: {注释列表.Count}");
                Console.WriteLine($"节点注释映射数量: {节点注释映射.Count}");
                Console.WriteLine($"属性注释映射数量: {属性注释映射.Count}");
                Console.WriteLine($"方法注释映射数量: {方法注释映射.Count}");
                
                foreach (var 映射 in 节点注释映射)
                {
                    Console.WriteLine($"节点 {映射.Key} 有 {映射.Value.Count} 个注释");
                }
                
                foreach (var 映射 in 属性注释映射)
                {
                    Console.WriteLine($"属性 {映射.Key} 有 {映射.Value.Count} 个注释");
                }
                
                foreach (var 映射 in 方法注释映射)
                {
                    Console.WriteLine($"方法 {映射.Key} 有 {映射.Value.Count} 个注释");
                }
                
                var 生成的代码 = 代码生成器.生成代码(抽象语法树, 输入文件路径, 注释列表, 节点注释映射, 属性注释映射, 方法注释映射);
                Console.WriteLine("C#代码生成完成");
                Console.WriteLine("生成的C#代码预览：\n" + 生成的代码);
                return 生成的代码;
            }
            catch (Exception 异常)
            {
                Console.WriteLine($"编译流程异常: {异常.Message}");
                Console.WriteLine($"堆栈信息: {异常.StackTrace}");
                return string.Empty;
            }
        }
    }
}
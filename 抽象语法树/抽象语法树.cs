// 引入泛型集合命名空间，用于存储节点列表
using System;
using System.Collections.Generic;

namespace CnToCs.抽象语法树
{
    /// <summary>
    /// 语法树节点为抽象类，所有节点继承此类
    /// </summary>
    public abstract class 语法树节点{};

    /// <summary>
    /// 语法树的根节点，承载所有的子节点
    /// </summary>
    public class 根节点 : 语法树节点
    {
        public List<语法树节点>子节点列表{get;}
        public 根节点(List<语法树节点>子节点列表)
        {
            this.子节点列表=子节点列表;
        }
    };
    /// <summary>
    /// 类型节点：属性、参数或方法返回值的类型
    /// </summary>
    public class 类型节点: 语法树节点
    {
        public string 类型名称{get;}
        public bool 是否为泛型{get;}
        public string? 修饰符{get;}
        public List<类型节点> 泛型参数列表{get;}
        /// <summary>
        /// 泛型参数列表和修饰符不填则默认为空
        /// </summary>
        public 类型节点(string 类型名称,bool 是否为泛型=false,string?修饰符=null,List<类型节点>泛型参数列表=null)
        {
            this.类型名称=类型名称;
            this.是否为泛型=是否为泛型;
            if(泛型参数列表==null)
            {
                泛型参数列表=new List<类型节点>();
            }
            else
            {
                this.泛型参数列表=泛型参数列表;
            };
            this.修饰符=修饰符;
        }
    };

    /// <summary>
    /// 参数节点:方法或构造函数的参数
    /// </summary>
     public class 参数节点 : 语法树节点
    {
        public string 参数名称{get;}
        public 类型节点 参数类型{get;}
        public bool 是否可为空{get;}
        //参数默认为public访问修饰符
        public 参数节点(string 参数名称,类型节点 参数类型,bool 是否可为空=false)
        {
            this.参数名称=参数名称;
            this.参数类型=参数类型;
            this.是否可为空=是否可为空;
        }
    }
    /// <summary>
    /// 属性节点：类或接口的属性
    /// </summary>
    public class 属性节点 : 语法树节点
    {
        public string 名称 {get;}
        public 类型节点 类型{get;}
        public bool 是否可为空{get;}
        //属性默认为public访问修饰符
        public string 修饰符{get;}
        public 属性初始化值节点? 初始化值 {get;}
        /// <summary>
        /// 修饰符不填默认为public
        /// </summary>
        public 属性节点(string 名称,类型节点 类型,bool 是否可为空,string 修饰符 = "public", 属性初始化值节点? 初始化值 = null)
        {
            this.名称=名称;
            this.类型=类型;
            this.是否可为空=是否可为空;
            this.修饰符=修饰符;
            this.初始化值 = 初始化值;
        }
    }
    /// <summary>
    /// 方法节点：C#中的函数，方法，构造函数等等节点
    /// </summary>
    public class 方法节点 : 语法树节点
    {
        public string 名称{get;}
        public string 返回类型{get;}
        public bool 是否为虚方法{get;}
        public bool 是否为抽象方法{get;}
        public bool 是否为重写方法{get;}
        public List<参数节点> 参数列表{get;}
        public 方法体节点? 方法体{get;}
 
        //参数列表不填默认参数列表为空，是否为虚方法不填默认为非虚方法
        public 方法节点(string 函数名称, 类型节点 返回类型, List<参数节点> 参数列表 = null, bool 是否为虚方法 = false, bool 是否为抽象方法 = false, bool 是否为重写方法 = false, 方法体节点? 方法体 = null)
        {
            this.名称 = 函数名称;
            this.返回类型 = 返回类型?.类型名称 ?? "void";
            if (参数列表 == null)
            {
                this.参数列表 = new List<参数节点>();
            }
            else
            {
                this.参数列表 = 参数列表;
            }
            this.是否为虚方法 = 是否为虚方法;
            this.是否为抽象方法 = 是否为抽象方法;
            this.是否为重写方法 = 是否为重写方法;
            this.方法体 = 方法体;
        }
    }
    /// <summary>
    /// 构造函数节点，表示类的构造函数
    /// </summary>
    public class 构造函数节点 : 语法树节点
    {
        public List<参数节点>参数列表{get;}
        public 方法体节点? 方法体{get;}
        public 构造函数节点(List<参数节点> 参数列表 = null, 方法体节点? 方法体 = null)
        {
            if (参数列表 == null)
            {
                this.参数列表 = new List<参数节点>();
            }
            else
            {
                this.参数列表 = 参数列表;
            }
            this.方法体 = 方法体;
        }
    }
    /// <summary>
    /// 类节点:类定义(包括类名，属性，方法，构造函数等)==class
    /// </summary>
    public class 类节点 : 语法树节点
    {
        public string 名称{get;}
        //如果有继承类则填写继承类名称，否则为null
        public string?基类名称{get;}
        public List<string> 基接口列表{get;}
        public List<属性节点> 属性列表{get;}
        public 构造函数节点? 构造函数{get;}
        public List<方法节点> 方法列表{get;}
        public 类节点(string 类名称, string? 基类名称 = null, List<string> 基接口列表 = null, List<属性节点> 属性列表 = null, List<方法节点> 方法列表 = null, 构造函数节点? 构造函数 = null)
        {
            this.名称 = 类名称;
            this.基类名称 = 基类名称;
            this.基接口列表 = 基接口列表 ?? new List<string>();
            if (属性列表 == null)
            {
                属性列表 = new List<属性节点>();
            }else
            {
                this.属性列表 = 属性列表;
            }
            if(方法列表 == null)
            {
                方法列表 = new List<方法节点>();
            }else
            {
                this.方法列表 = 方法列表;
            }
            this.构造函数 = 构造函数;
        }
    }

    /// <summary>
    /// 接口节点:一个C#接口定义 == interface
    /// </summary>
    public class 接口节点 : 语法树节点
    {
        public string 名称{get;}
        public List<string> 基接口列表{get;}
        public List<属性节点> 属性列表{get;}
        public List<方法节点> 方法列表{get;}
        public 接口节点(string 接口名称, List<属性节点> 属性列表, List<string> 基接口列表 = null, List<方法节点> 方法列表 = null)
        {
            this.名称 = 接口名称;
            this.基接口列表 = 基接口列表 ?? new List<string>();
            this.属性列表 = 属性列表;
           
            if(方法列表 == null)
            {
                方法列表 = new List<方法节点>();
            }else
            {
                this.方法列表 = 方法列表;
            }
        }
    }

    /// <summary>
    /// 语句节点：表示方法体中的语句
    /// </summary>
    public abstract class 语句节点 : 语法树节点
    {
    }

    /// <summary>
    /// 返回语句节点
    /// </summary>
    public class 返回语句节点 : 语句节点
    {
        public 表达式节点? 表达式 { get; }
        
        public 返回语句节点(表达式节点? 表达式 = null)
        {
            this.表达式 = 表达式;
        }
    }

    /// <summary>
    /// 表达式节点：表示一个表达式
    /// </summary>
    public abstract class 表达式节点 : 语法树节点
    {
    }

    /// <summary>
    /// 二元运算表达式节点
    /// </summary>
    public class 二元运算表达式节点 : 表达式节点
    {
        public 表达式节点 左操作数 { get; }
        public string 运算符 { get; }
        public 表达式节点 右操作数 { get; }
        
        public 二元运算表达式节点(表达式节点 左操作数, string 运算符, 表达式节点 右操作数)
        {
            this.左操作数 = 左操作数;
            this.运算符 = 运算符;
            this.右操作数 = 右操作数;
        }
    }

    /// <summary>
    /// 方法调用表达式节点
    /// </summary>
    public class 方法调用表达式节点 : 表达式节点
    {
        public 表达式节点 目标 { get; }
        public List<表达式节点> 参数列表 { get; }
        
        public 方法调用表达式节点(表达式节点 目标, List<表达式节点> 参数列表)
        {
            this.目标 = 目标;
            this.参数列表 = 参数列表 ?? new List<表达式节点>();
        }
    }

    /// <summary>
    /// 字面量表达式节点
    /// </summary>
    public class 字面量表达式节点 : 表达式节点
    {
        public string 值 { get; }
        public string 类型 { get; }
        
        public 字面量表达式节点(string 值, string 类型)
        {
            this.值 = 值;
            this.类型 = 类型;
        }
    }

    /// <summary>
    /// 标识符表达式节点
    /// </summary>
    public class 标识符表达式节点 : 表达式节点
    {
        public string 名称 { get; }
        
        public 标识符表达式节点(string 名称)
        {
            this.名称 = 名称;
        }
    }

    /// <summary>
    /// 属性访问表达式节点
    /// </summary>
    public class 属性访问表达式节点 : 表达式节点
    {
        public 表达式节点 目标 { get; }
        public string 属性名 { get; }
        
        public 属性访问表达式节点(表达式节点 目标, string 属性名)
        {
            this.目标 = 目标;
            this.属性名 = 属性名;
        }
    }

    /// <summary>
    /// 赋值语句节点
    /// </summary>
    public class 赋值语句节点 : 语句节点
    {
        public object 左侧 { get; } // 可以是字符串或表达式节点
        public 表达式节点 右侧 { get; }
        
        public 赋值语句节点(object 左侧, 表达式节点 右侧)
        {
            this.左侧 = 左侧;
            this.右侧 = 右侧;
        }
    }

    /// <summary>
    /// 如果语句节点（条件语句）
    /// </summary>
    public class 如果语句节点 : 语句节点
    {
        public 表达式节点 条件 { get; }
        public List<语句节点> Then语句列表 { get; }
        public List<语句节点> Else语句列表 { get; }
        
        public 如果语句节点(表达式节点 条件, List<语句节点> then语句列表, List<语句节点> else语句列表 = null)
        {
            this.条件 = 条件;
            this.Then语句列表 = then语句列表 ?? new List<语句节点>();
            this.Else语句列表 = else语句列表 ?? new List<语句节点>();
        }
    }

    /// <summary>
    /// 方法体节点：包含方法中的所有语句
    /// </summary>
    public class 方法体节点 : 语法树节点
    {
        public List<语句节点> 语句列表 { get; }
        
        public 方法体节点(List<语句节点> 语句列表)
        {
            this.语句列表 = 语句列表 ?? new List<语句节点>();
        }
    }

    /// <summary>
    /// 属性初始化值节点
    /// </summary>
    public class 属性初始化值节点 : 语法树节点
    {
        public 表达式节点 表达式 { get; }
        
        public 属性初始化值节点(表达式节点 表达式)
        {
            this.表达式 = 表达式;
        }
    }
}
// 此文件由中文转C#编译器自动生成
// 请勿手动修改此文件

using System;
using System.Collections.Generic;

namespace Generated_简单测试
{

        // 简单测试文件 - 测试基本的接口、类继承和方法重写功能
        /**
         * 定义一个基础接口A，包含一个属性和两个方法
         * 用于测试接口定义和实现
         */

    public interface A
    {
        // 公开属性：名称
        public string 名称 { get; set; }
        // 虚方法：获取信息，子类可以重写
        string 获取信息();
        // 普通方法：计算，返回整数
        int 计算();
    }


        /**
         * 基础实现类，实现接口A
         * 测试接口的基本实现和方法重写
         */

    public class 基础实现 : A
    {
        // 实现接口的属性
        public string 名称 { get; set; }
        // 私有属性，用于内部计数
        private int 计数 { get; set; }

        public 基础实现(string 名称)

        {
            this.名称 = 名称;
            this.计数 = 0;
        }

        // 构造函数，初始化名称和计数
        // 重写接口的虚方法，提供具体实现
        public virtual string 获取信息()
        {
            return ("基础实现: " + 名称);
        }
        // 实现接口的计算方法，每次调用计数加1
        public virtual int 计算()
        {
            this.计数 = (this.计数 + 1);
            return this.计数;
        }
    }


        /**
         * 高级实现类，继承自基础实现类
         * 测试类的继承、方法重写和基类方法调用
         */

    public class 高级实现 : 基础实现
    {
        // 新增公开属性：版本
        public string 版本 { get; set; }

        public 高级实现(string 名称, string 版本)
     : base(名称)
        {
            this.版本 = 版本;
        }

        // 构造函数，调用基类构造函数并初始化版本
        // 重写基类的获取信息方法，添加版本信息
        public override string 获取信息()
        {
            return (base.获取信息() + (", 版本: " + 版本));
        }
        // 新增方法，获取详细信息，演示如何访问基类属性和方法
        public string 获取详细信息()
        {
            return ("名称: " + (名称 + (", 版本: " + (版本 + (", 计数: " + base.计算())))));
        }
    }


}

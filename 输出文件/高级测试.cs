// 此文件由中文转C#编译器自动生成
// 请勿手动修改此文件

using System;
using System.Collections.Generic;

namespace Generated_高级测试
{

        // 高级测试文件 - 测试接口继承、复杂类层次结构和变量声明
        /**
         * 基础绘图接口，定义所有可绘制对象的基本属性
         * 包含位置坐标和绘制方法
         */

    public interface I可绘制
    {
        // X坐标
        public int X { get; set; }
        // Y坐标
        public int Y { get; set; }
        // 绘制方法，无参数
        string 绘制();
    }


        /**
         * 可移动接口，继承自可绘制接口
         * 扩展了移动功能
         */

    public interface I可移动 : I可绘制
    {
        // 移动方法，接受新的坐标
        void 移动(int 新X, int 新Y);
    }


        /**
         * 图形基类，实现可移动接口
         * 提供所有图形的基本功能
         */

    public class 图形 : I可移动
    {
        // 实现接口的X坐标属性
        public int X { get; set; }
        // 实现接口的Y坐标属性
        public int Y { get; set; }
        // 私有属性，存储图形颜色
        private string 颜色 { get; set; }

        public 图形(int X, int Y, string 颜色)

        {
            this.X = X;
            this.Y = Y;
            this.颜色 = 颜色;
        }

        // 构造函数，初始化位置和颜色
        // 实现接口的绘制方法，返回绘制信息
        public virtual string 绘制()
        {
            return ("绘制图形在位置(" + (X + (", " + (Y + (")，颜色为" + 颜色)))));
        }
        // 实现接口的移动方法，更新位置
        public virtual void 移动(int 新X, int 新Y)
        {
            this.X = 新X;
            this.Y = 新Y;
        }
        // 计算到目标点的距离（简化版，返回距离平方）
        public virtual double 计算距离(int 目标X, int 目标Y)
        {
            var dx = (目标X - this.X);
            var dy = (目标Y - this.Y);
            var 距离平方 = (dx * (dx + (dy * dy)));
            return 距离平方;
        }
    }


        /**
         * 圆形类，继承自图形类
         * 演示如何扩展基类功能
         */

    public class 圆形 : 图形
    {
        // 私有属性，存储圆的半径
        private int 半径 { get; set; }

        public 圆形(int X, int Y, string 颜色, int 半径)
     : base(X, Y, 颜色)
        {
            this.半径 = 半径;
        }

        // 构造函数，调用基类构造函数并初始化半径
        // 重写绘制方法，添加圆形特有信息
        public override string 绘制()
        {
            return (base.绘制() + ("，这是一个半径为" + (半径 + "的圆形")));
        }
        // 计算圆的面积
        public double 计算面积()
        {
            return (3.14159 * (半径 * 半径));
        }
        // 计算圆的周长
        public double 计算周长()
        {
            return (2 * (3.14159 * 半径));
        }
    }


        /**
         * 矩形类，继承自图形类
         * 演示另一种图形的实现方式
         */

    public class 矩形 : 图形
    {
        // 私有属性，存储矩形的宽度
        private int 宽度 { get; set; }
        // 私有属性，存储矩形的高度
        private int 高度 { get; set; }

        public 矩形(int X, int Y, string 颜色, int 宽度, int 高度)
     : base(X, Y, 颜色)
        {
            this.宽度 = 宽度;
            this.高度 = 高度;
        }

        // 构造函数，调用基类构造函数并初始化宽度和高度
        // 重写绘制方法，添加矩形特有信息
        public override string 绘制()
        {
            return (base.绘制() + ("，这是一个宽度为" + (宽度 + ("，高度为" + (高度 + "的矩形")))));
        }
        // 计算矩形的面积
        public int 计算面积()
        {
            return (宽度 * 高度);
        }
        // 计算矩形的周长
        public int 计算周长()
        {
            return (2 * (宽度 + 高度));
        }
    }


}

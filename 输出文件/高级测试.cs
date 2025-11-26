// 此文件由中文转C#编译器自动生成
// 请勿手动修改此文件

using System;
using System.Collections.Generic;

namespace Generated_高级测试
{

    public interface I可绘制
    {
        public int X { get; set; }
        public int Y { get; set; }

        string 绘制();
    }


    public interface I可移动 : I可绘制
    {

        string 移动(int 新X, int 新Y);
    }


    public class 图形 : I可移动
    {
        public int X { get; set; }
        public int Y { get; set; }
        private string 颜色 { get; set; }

        public 图形(int X, int Y, string 颜色)

        {
            this.X = X;
            this.Y = Y;
            this.颜色 = 颜色;
        }

        public virtual string 绘制()
        {
            return ("绘制图形在位置(" + (X + (", " + (Y + (")，颜色为" + 颜色)))));
        }
        public virtual string 移动(int 新X, int 新Y)
        {
            this.X = 新X;
            this.Y = 新Y;
            return "图形已移动到新位置";
        }
    }


    public class 圆形 : 图形
    {
        private int 半径 { get; set; }

        public 圆形(int X, int Y, string 颜色, int 半径)
     : base(X, Y, 颜色)
        {
            this.半径 = 半径;
        }

        public override string 绘制()
        {
            return (base.绘制() + ("，这是一个半径为" + (半径 + "的圆形")));
        }
    }


    public class 矩形 : 图形
    {
        private int 宽度 { get; set; }
        private int 高度 { get; set; }

        public 矩形(int X, int Y, string 颜色, int 宽度, int 高度)
     : base(X, Y, 颜色)
        {
            this.宽度 = 宽度;
            this.高度 = 高度;
        }

        public override string 绘制()
        {
            return (base.绘制() + ("，这是一个宽度为" + (宽度 + ("，高度为" + (高度 + "的矩形")))));
        }
    }


}

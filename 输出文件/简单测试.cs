// 此文件由中文转C#编译器自动生成
// 请勿手动修改此文件

using System;
using System.Collections.Generic;

namespace Generated_简单测试
{

    public interface A
    {
        public string 名称 { get; set; }

        virtual string 获取信息();
        virtual int 计算();
    }


    public class 基础实现 : A
    {
        public string 名称 { get; set; }
        private int 计数 { get; set; }

        public 基础实现(string 名称)
     : base(名称)
        {
            this.名称 = 名称;
        }

        public override string 获取信息()
        {
            return ("基础实现: " + 名称);
        }
        public new int 计算()
        {
            this.计数 = (this.计数 + 1);
            return this.计数;
        }
    }


    public class 高级实现 : 基础实现
    {
        public string 版本 { get; set; }

        public 高级实现(string 名称, string 版本)
     : base(名称)
        {
            this.版本 = 版本;
        }

        public override string 获取信息()
        {
            return (base.获取信息() + (", 版本: " + 版本));
        }
    }


}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Yased_Api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu
    {
        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public string Name_EN { get; set; }
        public string Link { get; set; }
        public int Sort { get; set; }
        public string target { get; set; }
        public string Link_EN { get; set; }
        public List<Menu> submenu { get; internal set; }
    }
}

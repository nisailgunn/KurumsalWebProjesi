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
    
    public partial class Content
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Description_EN { get; set; }
        public string Slug { get; set; }
        public string Slug_EN { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Image { get; set; }
        public Nullable<int> Type { get; set; }
    }
}

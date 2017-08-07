using System;

namespace GameStore.WebUI.Models
{
    public class PagingInfo
    {
        //count of products
        public int TotalItems { get; set; }
        //count of products per page
        public int ItemsPerPage { get; set; }
        // number of page
        public int CurrentPage { get; set; }
        //total count of pages
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}
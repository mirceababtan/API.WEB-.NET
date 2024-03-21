using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;

namespace WebApi.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int[] Ratings { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

/*-Id - Guid(must be unique)
- Name - string
- Description - string
- Ratings - int[](array of ratings between 1 and 5)
- CreatedOn - Datetime(when it was added to the list)->automatically generated
*/
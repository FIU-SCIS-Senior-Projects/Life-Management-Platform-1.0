using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeManagement.Models
{
    interface ImageInterface
    {
         byte[] Bytes { get; set; }
        string ImageMimeType { get; set; }

    }
}

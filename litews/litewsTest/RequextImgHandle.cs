using System;
using System.Collections.Generic;
using System.Text;

namespace litewsTest
{
    public class RequextImgHandle : RequestTextHandle
    {

        public RequextImgHandle() : base("image")
        {
            base.sMimeType = "image/";
        }
    }
}

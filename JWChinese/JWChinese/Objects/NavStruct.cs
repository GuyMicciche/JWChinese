using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWChinese
{
    public struct NavStruct
    {
        public string Publication;
        public string MepsID;
        public string ArticleID;

        public NavStruct(string publication, string mepsId, string articleId)
        {
            this.Publication = publication;
            this.MepsID = mepsId;
            this.ArticleID = articleId;
        }

        /// <summary>
        /// Creates an article navigation object
        /// </summary>
        /// <param name="nav"></param>
        /// <returns>Article nagivation object</returns>
        public static NavStruct Parse(string nav)
        {
            // In this format:  1.1.1
            char[] chrArray = new char[] { '.' };
            string[] strArrays = nav.Split(chrArray);
            if ((int)strArrays.Length != 3)
            {
                throw new ArgumentException("Invalid format of Verse string => " + strArrays.Length + " character(s) => " + nav + " is the name.");
            }

            return new NavStruct(strArrays[0], strArrays[1], strArrays[2]);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", Publication, MepsID, ArticleID);
        }
    }
}

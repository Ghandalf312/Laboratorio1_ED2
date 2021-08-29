using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;

namespace api
{
    public class Singleton
    {
        private static Singleton _instance;
        public static Singleton Instance
        {
            get
            {
                if (_instance == null) _instance = new Singleton();
                return _instance;
            }
        }
        public BTree<Movie> Tree;
        public string path;
    }
}

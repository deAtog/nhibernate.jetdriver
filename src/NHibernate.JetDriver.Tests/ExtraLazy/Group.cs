using System;
using System.Collections;
using System.Collections.Generic;

namespace NHibernate.JetDriver.Tests.ExtraLazy
{
    public class Group
    {
        private string name;
        private IDictionary<string, User> users = new Dictionary<string, User>();
        protected Group() { }
        public Group(string name)
        {
            this.name = name;
        }

        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        public virtual IDictionary<string, User> Users
        {
            get { return users; }
            set { users = value; }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

namespace NHibernate.JetDriver.Tests.ExtraLazy
{
    public class User
    {
        private string name;
        private string _passwordValue;
        private IDictionary<string, SessionAttribute> session = new Dictionary<string, SessionAttribute>();
        private ISet<Document> documents = new HashSet<Document>();
        private ISet<Photo> photos = new HashSet<Photo>();
        protected User() {}
        public User(string name, string passwordValue)
        {
            this.name = name;
            this._passwordValue = passwordValue;
        }

        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        public virtual string PasswordValue
        {
            get { return _passwordValue; }
            set { _passwordValue = value; }
        }

        public virtual IDictionary<string, SessionAttribute> Session
        {
            get { return session; }
            set { session = value; }
        }

        public virtual ISet<Document> Documents
        {
            get { return documents; }
            set { documents = value; }
        }


        public virtual ISet<Photo> Photos
        {
            get { return photos; }
            set { photos = value; }
        }
    }
}
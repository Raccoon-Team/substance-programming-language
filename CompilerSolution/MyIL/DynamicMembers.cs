using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IL2MSIL
{
    public class DynamicMembers
    {
        private Dictionary<string, List<MemberInfo>> members;

        private DynamicMembers()
        {
            members = new Dictionary<string, List<MemberInfo>>();
        }

        private static DynamicMembers instance;
        public static DynamicMembers GetInstance()
        {
            if (instance == null)
                instance = new DynamicMembers();
            return instance;
        }

        public List<MemberInfo> GetMembers(string typeName)
        {
            return members[typeName];
        }

        public void AddMember(string typeName, MemberInfo member)
        {
            if (!members.ContainsKey(typeName))
                members[typeName] = new List<MemberInfo>();
            members[typeName].Add(member);
        }
    }
}

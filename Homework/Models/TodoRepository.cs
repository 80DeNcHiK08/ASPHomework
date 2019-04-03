using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;

namespace Homework.Models
{
    public class TodoRepository : ITodoRepository
    {
        private static string path = @"../Homework/Static/TodoStorage.json";
        private static ConcurrentDictionary<string, TodoItem> _todos =
            new ConcurrentDictionary<string, TodoItem>();

        public IEnumerable<TodoItem> GetAll()
        {
            _todos = JsonConvert.DeserializeObject<ConcurrentDictionary<string, TodoItem>>(GetJson(path));
            return _todos.Values;
        }

        public void Add(TodoItem item)
        {
            item.TodoId = Guid.NewGuid().ToString();
            _todos[item.TodoId] = item;
            string addjson = JsonConvert.SerializeObject(_todos);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(addjson + "\n");
            }
        }

        public TodoItem Find(string key)
        {
            TodoItem item;
            _todos.TryGetValue(key, out item);
            return item;
        }

        public TodoItem Remove(string key)
        {
            TodoItem item;
            _todos.TryGetValue(key, out item);
            string source = GetJson(path);
            string result = string.Empty;
            int i = source.IndexOf(key);
            int RLength = (2 * key.Length) + item.TodoName.Length + item.IsComplete.ToString().Length + 48;
            if(i >= 0)
            {
                result = source.Remove(i-1, RLength);
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(result + "\n");
                }
            }
            _todos.TryRemove(key, out item);

            return item;
        }

        public void Update(TodoItem item, string Name)
        {
            _todos[item.TodoId] = item;
            var updatedstr = GetJson(path).Replace(_todos[item.TodoId].TodoName, Name);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(updatedstr + "\n");
            }
        }

        public static string GetJson(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }
    }
}

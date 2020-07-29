using System.Collections.Generic;

namespace Npgg.ApiRouter
{
    public class RouterNode<T> where T : class
    {
        public readonly string word;
        public readonly string fullPath;
        public Dictionary<string, RouterNode<T>> childrun { get; set; }
        public T Value { get; private set; }
        public RouterNode()
        {
            this.childrun = new Dictionary<string, RouterNode<T>>();
        }

        public RouterNode(int depth, string key, string fullPath)
        {
            this.word = key;
            this.fullPath = fullPath;
            this.childrun = new Dictionary<string, RouterNode<T>>();
        }

        const string _parameterWord = "{p}";

        public bool TryGet(string[] block, int depth, out RouterNode<T> result)
        {
            var word = block[depth];

            if (this.childrun.TryGetValue(word.ToLower(), out result) == false)
            {
                if (this.childrun.TryGetValue(_parameterWord, out result) == false)
                {
                    return false;
                }
            }

            depth++;

            if (depth < block.Length)
            {
                return result.TryGet(block, depth, out result);
            }
            else
            {
                return true;
            }
        }

        public void Add(string[] block, int depth, T value)
        {
            var currentWord = block[depth];
            var replacedWord = RouteHelper.IsParameter(currentWord) ? _parameterWord : currentWord.ToLower();
            if (childrun.TryGetValue(replacedWord, out var child) == false)
            {
                string fullPath = string.Join('/', block, 0, depth + 1);
                child = new RouterNode<T>(depth, replacedWord, fullPath);
                
                this.childrun.Add(child.word, child);
            }

            depth++;

            if(depth < block.Length)
            {
                child.Add(block, depth, value);
            }
            else
            {
                child.Value = value;
            }
        }

        public void ReadNode(Dictionary<string, T> result)
        {
            if(this.Value != null)
            {
                result.Add(this.fullPath, this.Value);
            }

            foreach(var child in childrun)
            {
                child.Value.ReadNode(result);
            }
        }
    }
}

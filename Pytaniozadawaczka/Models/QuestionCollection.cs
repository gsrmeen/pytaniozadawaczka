using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Pytaniozadawaczka.Models
{
    public class QuestionCollection : ICollection<Question>
	{
        public List<Question> Questions { get; }
        public QuestionCollection(List<Question> l)
		{
			Questions = l;
		}
		public QuestionCollection(string xmlPath)
		{
			Questions = new List<Question>();
			XDocument doc = XDocument.Load(xmlPath);
			foreach (XElement elem in doc.Elements().First().Elements())
			{
				Question q = new Question(elem);
				Questions.Add(q);
			}
		}
		

		public int Count => Questions.Count;

		public bool IsReadOnly => false;

		public void Add(Question item)
		{
			Questions.Add(item);
		}

		public void Clear()
		{
			Questions.Clear();
		}

		public bool Contains(Question item)
		{
			return Questions.Contains(item);
		}

		public void CopyTo(Question[] array, int arrayIndex)
		{
			Questions.CopyTo(array, arrayIndex);
		}

		public IEnumerator<Question> GetEnumerator()
		{
			return Questions.GetEnumerator();
		}

		public bool Remove(Question item)
		{
			return Questions.Remove(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Questions.GetEnumerator();
		}
	}
}

using System.Collections.Generic;
using System.Xml.Linq;

namespace Pytaniozadawaczka.Models
{
    public class Question
	{
		public string Value { get; set; }
		private List<Answer> mAnswers;

		public List<Answer> Answers { get { return mAnswers;} }

		public Question(string val, List<Answer> ans)
		{
			Value = val;
			mAnswers = ans;
		}



		public Question(XElement element)
		{
			mAnswers = new List<Answer>();
			Value = element.Attribute("value").Value;

			foreach (XElement aElem in element.Elements())
			{
				Answer a = new Answer();
				a.Value = aElem.Attribute("value").Value;
				if (aElem.Attribute("correct") != null && aElem.Attribute("correct").Value == "true")
				{
					a.Correct = true;
				}
				else
				{
					a.Correct = false;
				}
				mAnswers.Add(a);
			}
		}





	}
}

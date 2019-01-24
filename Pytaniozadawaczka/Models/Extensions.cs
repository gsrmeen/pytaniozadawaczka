using System;
using System.Collections.Generic;
using System.Linq;

namespace Pytaniozadawaczka.Models
{
    public static class Extensions
	{
		public static Question RandomizeAnswers(this Question main)
		{
			Answer[] tempAns = new Answer[main.Answers.Count];
			main.Answers.CopyTo(tempAns);
			List<Answer> tempList = tempAns.ToList();
			tempList = tempList.OrderBy(a => Guid.NewGuid()).ToList();
			return new Question(main.Value, tempList);
		}



		public static QuestionCollection RandomizeCollection(this QuestionCollection whole, int howMany)
		{
			if (howMany > whole.Count) throw new Exception();

			List<Question> tempList = whole.Questions.AsEnumerable().OrderBy(n => Guid.NewGuid()).Take(howMany).ToList();

			for (int i = 0; i < tempList.Count; i++)
			{
				tempList[i] = tempList[i].RandomizeAnswers();
			}


			return new QuestionCollection(tempList);
		}




		public static (bool, bool[] correctAnswer) VerifyAnswer(this Question q, bool[] solution)
		{
			if (q.Answers.Count != solution.Length) throw new Exception();

			bool toret = true;
			bool[] correctAnswer = new bool[solution.Length];
			for (int i = 0; i < solution.Length; i++)
			{
				if (q.Answers[i].Correct != solution[i])
				{
					toret = false;
					correctAnswer[i] = false;
				}
				else
				{
					correctAnswer[i] = true;
				}
			}

			return (toret, correctAnswer);
		}
	}
}

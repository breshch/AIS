using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Helpers;
using AIS_Enterprise_Global.Helpers;

namespace AVRepository.Repositories
{
	public class ParameterRepository : BaseRepository
	{

		public void EditParameter<T>(ParameterType parameterType, T value)
		{
			using (var db = GetContext())
			{
				db.Parameters.First(p => p.Name == parameterType.ToString()).Value = value.ToString();
				db.SaveChanges();
			}
		}

		public T GetParameterValue<T>(ParameterType parameterType)
		{
			using (var db = GetContext())
			{
				var parameter = db.Parameters.FirstOrDefault(p => p.Name == parameterType.ToString());
				if (parameter == null)
				{
					parameter = db.Parameters.Add(new Parameter
					{
						Name = parameterType.ToString(),
						Value = default(T).ToString()
					});
					db.SaveChanges();
				}
				db.Entry(parameter).Reload();

				string value = parameter.Value;

				return (T)Convert.ChangeType(value, typeof(T));
			}
		}
	}
}

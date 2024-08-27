//хз зачем всё это
using System.Reflection;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
sealed class ToDisposeAttribute: Attribute;
interface IFieldDisposing: IDisposable
{
	static readonly private Dictionary<Type, IEnumerable<FieldInfo>> _memo = new();
	void IDisposable.Dispose()
	{
		if (_memo.TryGetValue(GetType(), out var fields))
		{
			foreach (var field in fields) (field.GetValue(this) as IDisposable).Dispose();
		}
		else
		{
			var newfields = from field in GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic) 
							where field?.GetCustomAttribute(typeof(ToDisposeAttribute)) is not null
							select field;
			_memo.Add(GetType(), newfields);
			
			foreach (var field in newfields)
					(field.GetValue(this) as IDisposable).Dispose();
		}
	}
}

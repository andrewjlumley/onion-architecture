using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
	public abstract class DomainBase<TDomainObject> : IDomainObject
		where TDomainObject : DomainBase<TDomainObject>
	{
		#region Setter

		public bool Setter<TPropertyType>(Func<TPropertyType> get, Action<TPropertyType> set, TPropertyType value, String propertyName)
		{
			// Get the current value for comparison.
			TPropertyType current = get();

			// If both are null, then no change.
			if (value == null && current == null)
				return false;

			// If both are equal, then no change.
			if (value != null && value.Equals(current))
				return false;

			// Object is guaranteed to be IsUpdateable, and value is changing. Assign new value.
			set(value);

			// Trigger methods on property change
			TriggerRecalculateOnPropertyChange(propertyName);

			// Trigger notifications changes on property change
			TriggerNotificationOnPropertyChange(propertyName);

			return true;
		}



		#endregion

		#region Change Handling

		private void TriggerRecalculateOnPropertyChange(String propertyName)
		{
			var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

			foreach (var method in methods)
			{
				foreach (NotifiedByAttribute attrib in method.GetCustomAttributes(typeof(NotifiedByAttribute), true))
				{
					if (attrib.Properties.Contains(propertyName))
					{
						method.Invoke(null, new[] { (TDomainObject)this });
					}
				}
			}
		}

		#endregion

		#region Notifications

		public event PropertyChangedEventHandler? PropertyChanged;

		private void TriggerNotificationOnPropertyChange(String propertyName)
		{
			// Flag change on named property
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

			// Flag notification on linked properties
			var properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

			foreach (var property in properties)
			{
				foreach (NotifiedByAttribute attrib in property.GetCustomAttributes(typeof(NotifiedByAttribute), true))
				{
					if (attrib.Properties.Contains(propertyName))
					{
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.Name));
					}
				}
			}
		}

		#endregion

		#region Validation

		public bool TryIsValid(out ValidationResult[] violations)
		{
			var results = new Collection<ValidationResult>();
			var status = Validator.TryValidateObject(this, new ValidationContext(this), results, true);
			violations = results.ToArray();
			return status;
		}

		#endregion
	}
}

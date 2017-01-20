//
//  Copyright (C) 2017 Fluendo S.A.
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
using System;
using System.ComponentModel;
using VAS.Core.License;
using VAS.Core.MVVMC;

namespace VAS.Core.ViewModel
{
	/// <summary>
	/// LicenseLimitation ViewModel.
	/// </summary>
	public class LicenseLimitationVM<T> : ViewModelBase<T>
		where T : LicenseLimitation
	{
		Command upgradeCommand;

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public override T Model {
			get {
				return base.Model;
			}
			set {
				if (base.Model != null) {
					base.Model.PropertyChanged -= HandleModelChanged;
				}
				base.Model = value;
				if (base.Model != null) {
					base.Model.PropertyChanged += HandleModelChanged;
				}
			}
		}

		/// <summary>
		/// Proxy property for LimitationNmae.
		/// </summary>
		/// <value>The name of the limitation.</value>
		public string LimitationName {
			get {
				return Model?.LimitationName;
			}
		}

		/// <summary>
		/// Proxy property for Maximum.
		/// </summary>
		/// <value>The maximum.</value>
		public int Maximum {
			get {
				return Model?.Maximum ?? 0;
			}
		}

		/// <summary>
		/// Proxy property for Enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		public bool Enabled {
			get {
				return Model?.Enabled ?? false;
			}
		}

		/// <summary>
		/// Command to upgrade away from this limitation.
		/// </summary>
		/// <value>The upgrade command.</value>
		public Command UpgradeCommand {
			get {
				return upgradeCommand;
			}

			set {
				upgradeCommand = value;
				upgradeCommand.Executable = Enabled;
			}
		}

		void HandleModelChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof (Model.Enabled)) {
				UpgradeCommand.Executable = Enabled;
			}
		}
	}
}

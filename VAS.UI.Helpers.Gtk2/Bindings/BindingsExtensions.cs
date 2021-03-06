﻿//
//  Copyright (C) 2016 Fluendo S.A.
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
using System.Linq.Expressions;
using Gtk;
using VAS.Core.Common;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.MVVMC;
using Image = VAS.Core.Common.Image;

namespace VAS.UI.Helpers.Bindings
{
	public static class BindingsExtensions
	{
		/// <summary>
		/// Gets the binding context for a given widget and creates it if doesn't exist.
		/// </summary>
		/// <returns>The binding context.</returns>
		/// <param name="widget">Widget.</param>
		public static BindingContext GetBindingContext (this Widget widget)
		{
			if (widget.Data.ContainsKey ("Bindings")) {
				return widget.Data ["Bindings"] as BindingContext;
			}
			var context = new BindingContext ();
			widget.Data ["Bindings"] = context;
			return context;
		}

		/// <summary>
		/// Bind the specified label to a property by name.
		/// </summary>
		/// <param name="label">Label.</param>
		public static LabelBinding<TSourceProperty> Bind<TSourceProperty> (this Label label, Expression<Func<IViewModel, TSourceProperty>> sourcePropertyExpression)
		{
			return new LabelBinding<TSourceProperty> (label, sourcePropertyExpression);
		}

		/// <summary>
		/// Bind the specified label to a property by name using a converter.
		/// </summary>
		/// <returns>The bind.</returns>
		/// <param name="label">Label.</param>
		/// <param name="sourcePropertyExpression">Property expression.</param>
		/// <param name="converter">Converter.</param>
		public static LabelBinding<TSourceProperty> Bind<TSourceProperty> (this Label label, Expression<Func<IViewModel, TSourceProperty>> sourcePropertyExpression, TypeConverter converter)
		{
			return new LabelBinding<TSourceProperty> (label, sourcePropertyExpression, converter);
		}

		/// <summary>
		/// Bind the specified text view to a property by name.
		/// </summary>
		/// <param name="textView">Text view.</param>
		/// <param name="propertyName">Property name.</param>
		public static TextViewBinding Bind (this TextView textView, Expression<Func<IViewModel, string>> propertyExpression)
		{
			return new TextViewBinding (textView, propertyExpression);
		}

		/// <summary>
		/// Bind the specified entry to a property by name.
		/// </summary>
		/// <param name="entry">Entry.</param>
		public static EntryBinding Bind (this Entry entry, Expression<Func<IViewModel, string>> propertyExpression)
		{
			return new EntryBinding (entry, propertyExpression);
		}

		/// <summary>
		/// Bind the specified image to a property by name. The image is scalled to <paramref name="width"/> and
		/// <paramref name="height"/> if specified.
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="propertyExpression">Property expression.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static ImageBinding Bind (this ImageView image, Expression<Func<IViewModel, Image>> propertyExpression, int width = 0, int height = 0)
		{
			return new ImageBinding (image, propertyExpression, width, height);
		}

		/// <summary>
		/// Bind the specified toggle button to a command by name with the arguments to be passed when the button is
		/// activated and deactivated.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <param name="commandFunc">Command function.</param>
		/// <param name="parameterActive">Parameter when active.</param>
		/// <param name="parameterInactive">Parameter when inactive.</param>
		/// <param name="iconOnly">Ignore the Name property and only shows the icon in the button.</param>
		public static ToggleButtonBinding Bind (this ToggleButton button, Func<IViewModel, Command> commandFunc,
												object parameterActive, object parameterInactive, bool iconOnly = false)
		{
			return new ToggleButtonBinding (button, commandFunc, parameterActive, parameterInactive, iconOnly ? "" : null);
		}

		/// <summary>
		/// Bind the specified button to a command by name with the paramater to pass to the command.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <param name="commandFunc">Command function.</param>
		/// <param name="parameter">Parameter.</param>
		public static ButtonBinding Bind (this Button button, Func<IViewModel, Command> commandFunc, object parameter = null)
		{
			return new ButtonBinding (button, commandFunc, parameter);
		}

		/// <summary>
		/// Bind the specified button to a command by name with the paramater to pass to the command using a different
		/// than the one defined in the command.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <param name="image">Image.</param>
		/// <param name="commandFunc">Command name.</param>
		/// <param name="parameter">Parameter.</param>
		public static ButtonBinding BindWithIcon (this Button button, Image image, Func<IViewModel, Command> commandFunc, object parameter = null)
		{
			return new ButtonBinding (button, commandFunc, parameter, image, "");
		}

		/// <summary>
		/// Bind the specified CheckButton to a property by name.
		/// </summary>
		/// <param name="CheckButton">CheckButton.</param>
		public static CheckBoxBinding Bind (this CheckButton checkButton, Expression<Func<IViewModel, bool>> propertyExpression)
		{
			return new CheckBoxBinding (checkButton, propertyExpression);
		}

		/// <summary>
		/// Bind the specified SpinButton to a property by name.
		/// </summary>
		/// <param name="SpinButton">SpinButton.</param>
		public static SpinBinding<TSourceProperty> Bind<TSourceProperty> (this SpinButton spinButton, Expression<Func<IViewModel, TSourceProperty>> propertyExpression,
										TypeConverter converter = null)
		{
			return new SpinBinding<TSourceProperty> (spinButton, propertyExpression, converter);
		}

		/// <summary>
		/// Bind the specified colorButton and propertyExpression.
		/// </summary>
		/// <param name="colorButton">Color button.</param>
		/// <param name="propertyExpression">Property expression.</param>
		public static ColorButtonBinding Bind (this ColorButton colorButton, Expression<Func<IViewModel, Color>> propertyExpression)
		{
			return new ColorButtonBinding (colorButton, propertyExpression);
		}

		/// <summary>
		/// Bind the specified spinner to a property by name.
		/// </summary>
		/// <param name="spinner">Spinner.</param>
		public static SpinnerBinding Bind (this Gtk.Image spinner, Expression<Func<IViewModel, bool>> propertyExpression)
		{
			return new SpinnerBinding (spinner, propertyExpression);
		}

		/// <summary>
		/// Bind the specified ComboBox to a property by name.
		/// </summary>
		/// <param name="comboBox">ComboBox.</param>
		public static ComboBoxBinding<TSourceProperty> Bind<TSourceProperty> (this ComboBox comboBox, Expression<Func<IViewModel, TSourceProperty>> propertyExpression, TypeConverter converter = null)
		{
			return new ComboBoxBinding<TSourceProperty> (comboBox, propertyExpression, converter);
		}

		/// <summary>
		/// Bind the specified hscale, commandFunc and defaultValue.
		/// </summary>
		/// <returns>The bind.</returns>
		/// <param name="hscale">Hscale.</param>
		/// <param name="commandFunc">Command func.</param>
		/// <param name="defaultValue">Default value.</param>
		public static HScaleCommandBinding Bind (this HScale hscale, Func<IViewModel, Command<double>> commandFunc, double defaultValue)
		{
			return new HScaleCommandBinding (hscale, commandFunc, defaultValue);
		}

		/// <summary>
		/// Bind the specified hscale, commandFunc, defaultValue, min, max, step and page.
		/// </summary>
		/// <returns>The bind.</returns>
		/// <param name="hscale">Hscale.</param>
		/// <param name="commandFunc">Command func.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		/// <param name="step">Step.</param>
		/// <param name="page">Page.</param>
		public static HScaleCommandBinding Bind (this HScale hscale, Func<IViewModel, Command<double>> commandFunc, double defaultValue,
										 double min, double max, double step, double page)
		{
			return new HScaleCommandBinding (hscale, commandFunc, defaultValue, min, max, step, page);
		}

		/// <summary>
		/// Bind the specified hscale, commandFunc and defaultValue. For LimitationCommand<double>
		/// </summary>
		/// <returns>The bind.</returns>
		/// <param name="hscale">Hscale.</param>
		/// <param name="commandFunc">Command func.</param>
		/// <param name="defaultValue">Default value.</param>
		public static HScaleCommandBinding Bind (this HScale hscale, Func<IViewModel, LimitationCommand<double>> commandFunc,
										  double defaultValue)
		{
			return new HScaleCommandBinding (hscale, commandFunc, defaultValue);
		}

		/// <summary>
		/// Bind the specified hscale, commandFunc, defaultValue, min, max, step and page.
		/// For LimitationCommand<double>
		/// </summary>
		/// <returns>The bind.</returns>
		/// <param name="hscale">Hscale.</param>
		/// <param name="commandFunc">Command func.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		/// <param name="step">Step.</param>
		/// <param name="page">Page.</param>
		public static HScaleCommandBinding Bind (this HScale hscale, Func<IViewModel, LimitationCommand<double>> commandFunc,
										  double defaultValue, double min, double max, double step, double page)
		{
			return new HScaleCommandBinding (hscale, commandFunc, defaultValue, min, max, step, page);
		}
	}
}
using System;
using System.ComponentModel;
using System.Windows;
using ICSharpCode.AvalonEdit;

namespace Grepper.Controls
{

    public class BindableTextEditor : TextEditor
    {
        public string BindText
        {
            get => (string)GetValue(BindTextProperty);
            set => SetValue(BindTextProperty, value);
        }

        public static DependencyProperty BindTextProperty =
            DependencyProperty.Register(nameof(BindText), typeof(string), typeof(BindableTextEditor),
                new PropertyMetadata(string.Empty, (s, e) =>
                {
                    var target = (BindableTextEditor)s;
                    if (target.Text != (string)e.NewValue)
                        target.Text = (string)e.NewValue;
                })
            );
    }
}

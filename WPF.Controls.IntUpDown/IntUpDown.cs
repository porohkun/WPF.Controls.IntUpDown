using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPF.Controls
{
    [TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_ButtonIncrement, Type = typeof(Button))]
    [TemplatePart(Name = PART_ButtonDecrement, Type = typeof(Button))]
    public class IntUpDown : Control
    {
        #region Template parts

        internal const string PART_TextBox = "PART_TextBox";
        internal const string PART_ButtonIncrement = "PART_ButtonIncrement";
        internal const string PART_ButtonDecrement = "PART_ButtonDecrement";

        protected TextBox TextBox { get; private set; }
        protected Button IncrementButton { get; private set; }
        protected Button DecrementButton { get; private set; }

        #endregion //Template parts

        #region Proxy properties

        #region SelectionBrush

        public static readonly DependencyProperty SelectionBrushProperty
            = DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(IntUpDown), new UIPropertyMetadata(Brushes.Blue));

        [Bindable(true)]
        [Category("Appearance")]
        public Brush SelectionBrush
        {
            get => (Brush)GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

        #endregion //SelectionBrush

        #region TextAlignment

        public static readonly DependencyProperty TextAlignmentProperty
            = DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(IntUpDown), new UIPropertyMetadata(TextAlignment.Left));

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }


        #endregion //TextAlignment

        #endregion //Proxy properties

        #region Properties

        #region AllowTextInput

        public static readonly DependencyProperty AllowTextInputProperty
            = DependencyProperty.Register(nameof(AllowTextInput), typeof(bool), typeof(IntUpDown), new UIPropertyMetadata(true, OnAllowTextInputChanged));

        public bool AllowTextInput
        {
            get => (bool)GetValue(AllowTextInputProperty);
            set => SetValue(AllowTextInputProperty, value);
        }

        private static void OnAllowTextInputChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is IntUpDown s)
                s.OnAllowTextInputChanged((bool)args.OldValue, (bool)args.NewValue);
        }

        protected virtual void OnAllowTextInputChanged(bool oldValue, bool newValue)
        {
        }

        #endregion //AllowTextInput

        #region IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty
            = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(IntUpDown), new UIPropertyMetadata(false, OnIsReadOnlyChanged));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        private static void OnIsReadOnlyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is IntUpDown s)
                s.OnIsReadOnlyChanged((bool)args.OldValue, (bool)args.NewValue);
        }

        protected virtual void OnIsReadOnlyChanged(bool oldValue, bool newValue)
        {
        }

        #endregion //IsReadOnly

        #region ClampValueToMinMax

        public static readonly DependencyProperty ClipValueToMinMaxProperty
            = DependencyProperty.Register(nameof(ClampValueToMinMax), typeof(bool), typeof(IntUpDown), new UIPropertyMetadata(true, OnClipValueToMinMaxChanged));

        public bool ClampValueToMinMax
        {
            get => (bool)GetValue(ClipValueToMinMaxProperty);
            set => SetValue(ClipValueToMinMaxProperty, value);
        }

        private static void OnClipValueToMinMaxChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is IntUpDown s)
                s.OnClipValueToMinMaxChanged((bool)args.OldValue, (bool)args.NewValue);
        }

        protected virtual void OnClipValueToMinMaxChanged(bool oldValue, bool newValue)
        {
        }

        #endregion //ClampValueToMinMax

        #region MaxValue

        public static readonly DependencyProperty MaxValueProperty
            = DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(IntUpDown), new UIPropertyMetadata(int.MaxValue, OnMaxValueChanged));

        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        private static void OnMaxValueChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is IntUpDown s)
                s.OnMaxValueChanged((int)args.OldValue, (int)args.NewValue);
        }

        protected virtual void OnMaxValueChanged(int oldValue, int newValue)
        {
            if (ClampValueToMinMax)
                Value = ClampValue(Value);
        }

        #endregion //MaxValue

        #region MinValue

        public static readonly DependencyProperty MinValueProperty
            = DependencyProperty.Register(nameof(MinValue), typeof(int), typeof(IntUpDown), new UIPropertyMetadata(int.MinValue, OnMinValueChanged));

        public int MinValue
        {
            get => (int)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        private static void OnMinValueChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is IntUpDown s)
                s.OnMinValueChanged((int)args.OldValue, (int)args.NewValue);
        }

        protected virtual void OnMinValueChanged(int oldValue, int newValue)
        {
            if (ClampValueToMinMax)
                Value = ClampValue(Value);
        }

        #endregion //MinValue

        #region Value

        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register(nameof(Value), typeof(int), typeof(IntUpDown), new FrameworkPropertyMetadata(default(int), OnValueChanged) { BindsTwoWayByDefault = true });

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is IntUpDown s)
                s.OnValueChanged((int)args.OldValue, (int)args.NewValue);
        }

        protected virtual void OnValueChanged(int oldValue, int newValue)
        {
            if (!IsInitialized)
                return;

            if (ClampValueToMinMax)
                Value = ClampValue(Value);

            if (TextBox != null)
                TextBox.Text = ConvertValueToText(Value);
        }

        #endregion //Value

        #region Step

        public static readonly DependencyProperty StepProperty
            = DependencyProperty.Register(nameof(Step), typeof(int), typeof(IntUpDown), new PropertyMetadata(1, OnStepChanged));

        public int Step
        {
            get => (int)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        private static void OnStepChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntUpDown intUpDown)
                intUpDown.OnStepChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual void OnStepChanged(int oldValue, int newValue)
        {
        }

        #endregion //Step

        #endregion //Properties

        #region Constructors

        public IntUpDown()
        {
            IsKeyboardFocusWithinChanged += UpDownBase_IsKeyboardFocusWithinChanged;
        }

        #endregion //Constructors

        #region Base Class Overrides

        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            if (TextBox != null)
                TextBox.Focus();

            base.OnAccessKey(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (TextBox != null)
            {
                TextBox.PreviewMouseLeftButtonDown -= TextBox_SelectivelyIgnoreMouseButton;
                TextBox.GotKeyboardFocus -= TextBox_SelectAllText;
                TextBox.MouseDoubleClick -= TextBox_SelectAllText;
            }

            TextBox = GetTemplateChild(PART_TextBox) as TextBox;

            if (TextBox != null)
            {
                TextBox.Text = ConvertValueToText(Value);
                TextBox.PreviewMouseLeftButtonDown += TextBox_SelectivelyIgnoreMouseButton;
                TextBox.GotKeyboardFocus += TextBox_SelectAllText;
                TextBox.MouseDoubleClick += TextBox_SelectAllText;
            }



            if (IncrementButton != null)
                IncrementButton.Click -= IncrementButton_Clicked;

            IncrementButton = GetTemplateChild(PART_ButtonIncrement) as Button;

            if (IncrementButton != null)
                IncrementButton.Click += IncrementButton_Clicked;



            if (DecrementButton != null)
                DecrementButton.Click -= DecrementButton_Clicked;

            DecrementButton = GetTemplateChild(PART_ButtonDecrement) as Button;

            if (DecrementButton != null)
                DecrementButton.Click += DecrementButton_Clicked;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        CommitInput();
                        e.Handled = true;
                        break;
                    }
            }
        }

        #endregion //Base Class Overrides

        #region Methods

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            CommitInput();
        }

        internal void DoIncrement()
        {
            OnIncrement();
        }

        internal void DoDecrement()
        {
            OnDecrement();
        }

        protected void OnIncrement()
        {
            var result = IncrementValue(Value, Step);
            Value = ClampValue(result);
            TextBox.Text = ConvertValueToText(Value);
        }

        protected void OnDecrement()
        {
            var result = DecrementValue(Value, Step);
            Value = ClampValue(result);
            TextBox.Text = ConvertValueToText(Value);
        }

        protected int IncrementValue(int value, int increment)
        {
            return value + increment;
        }

        protected int DecrementValue(int value, int increment)
        {
            return value - increment;
        }

        private void TextBox_SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!textBox?.IsKeyboardFocusWithin ?? true)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }

        private void TextBox_SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox != null)
                textBox.SelectAll();
        }

        private void IncrementButton_Clicked(object sender, RoutedEventArgs e)
        {
            DoIncrement();
        }

        private void DecrementButton_Clicked(object sender, RoutedEventArgs e)
        {
            DoDecrement();
        }

        private void UpDownBase_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                CommitInput();
            }
        }

        public virtual void CommitInput()
        {
            if (TextBox == null)
                return;

            var newValue = ConvertTextToValue(TextBox.Text);
            if (newValue != Value)
                Value = newValue;
            TextBox.Text = ConvertValueToText(Value);
        }

        private int ClampValue(int value)
        {
            if (!ClampValueToMinMax)
                return value;
            if (value < MinValue)
                return MinValue;
            else if (value > MaxValue)
                return MaxValue;
            else
                return value;
        }

        protected int ConvertTextToValue(string text)
        {
            var result = 0;

            if (String.IsNullOrEmpty(text))
                return result;

            int.TryParse(text, out result);

            if (ClampValueToMinMax)
                return ClampValue(result);

            return result;
        }

        protected string ConvertValueToText(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion //Methods

    }
}

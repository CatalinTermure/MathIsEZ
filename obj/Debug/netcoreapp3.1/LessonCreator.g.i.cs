﻿#pragma checksum "..\..\..\LessonCreator.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0C4C0C4EDA6D0744D85D50EB966E570B735F955A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MathIsEZ;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MathIsEZ {
    
    
    /// <summary>
    /// LessonCreator
    /// </summary>
    public partial class LessonCreator : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\LessonCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas LessonCanvas;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\LessonCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnShow;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\LessonCreator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MathIsEZ.ShapeToolbar SToolbar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MathIsEZ;component/lessoncreator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\LessonCreator.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LessonCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 20 "..\..\..\LessonCreator.xaml"
            this.LessonCanvas.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.LessonCanvas_MouseDown);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\LessonCreator.xaml"
            this.LessonCanvas.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.LessonCanvas_MouseUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.BtnShow = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\LessonCreator.xaml"
            this.BtnShow.Click += new System.Windows.RoutedEventHandler(this.BtnShow_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SToolbar = ((MathIsEZ.ShapeToolbar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


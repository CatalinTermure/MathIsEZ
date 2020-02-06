﻿#pragma checksum "..\..\..\ShapeToolbar.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2A0ADEA0ABB63BD4D53FD08F50D396F49A4D445B"
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
    /// ShapeToolbar
    /// </summary>
    public partial class ShapeToolbar : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ToolbarGrid;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnEllipse;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnRectangle;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnTriangle;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnPolygon;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnGraph;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnText;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox FillCheckbox;
        
        #line default
        #line hidden
        
        
        #line 208 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Color1Btn;
        
        #line default
        #line hidden
        
        
        #line 211 "..\..\..\ShapeToolbar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Color2Btn;
        
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
            System.Uri resourceLocater = new System.Uri("/MathIsEZ;component/shapetoolbar.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ShapeToolbar.xaml"
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
            this.ToolbarGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            
            #line 32 "..\..\..\ShapeToolbar.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnHide_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BtnEllipse = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\ShapeToolbar.xaml"
            this.BtnEllipse.Click += new System.Windows.RoutedEventHandler(this.BtnEllipse_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.BtnRectangle = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\..\ShapeToolbar.xaml"
            this.BtnRectangle.Click += new System.Windows.RoutedEventHandler(this.BtnRectangle_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BtnTriangle = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\..\ShapeToolbar.xaml"
            this.BtnTriangle.Click += new System.Windows.RoutedEventHandler(this.BtnTriangle_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BtnPolygon = ((System.Windows.Controls.Button)(target));
            
            #line 92 "..\..\..\ShapeToolbar.xaml"
            this.BtnPolygon.Click += new System.Windows.RoutedEventHandler(this.BtnPolygon_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BtnGraph = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\..\ShapeToolbar.xaml"
            this.BtnGraph.Click += new System.Windows.RoutedEventHandler(this.BtnGraph_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.BtnText = ((System.Windows.Controls.Button)(target));
            
            #line 126 "..\..\..\ShapeToolbar.xaml"
            this.BtnText.Click += new System.Windows.RoutedEventHandler(this.BtnText_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.FillCheckbox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 169 "..\..\..\ShapeToolbar.xaml"
            this.FillCheckbox.Checked += new System.Windows.RoutedEventHandler(this.FillCheckbox_Checked);
            
            #line default
            #line hidden
            
            #line 169 "..\..\..\ShapeToolbar.xaml"
            this.FillCheckbox.Unchecked += new System.Windows.RoutedEventHandler(this.FillCheckbox_Unchecked);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Color1Btn = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.Color2Btn = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


using System;
using Android.Content;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

using AView = Android.Views.View;
using AApplication = Android.App.Application;

namespace VisualTreeElementImpExample.Android;

public class MyTemplatedControlHandler: ViewHandler<MyTemplatedControl, AView> {
        public static IPropertyMapper<MyTemplatedControl, MyTemplatedControlHandler> PropertyMapper = new PropertyMapper<MyTemplatedControl, MyTemplatedControlHandler>(ViewHandler.ViewMapper) {
            [nameof(MyTemplatedControl.ItemTemplate)] = MapItemTemplate
        };

        public static void MapItemTemplate(MyTemplatedControlHandler handler, MyTemplatedControl myTemplatedControl) {
            handler.UpdateTemplate(myTemplatedControl.ItemTemplate);
        }

        MauiViewGroup mauiViewGroup;

        public MyTemplatedControlHandler() : base(PropertyMapper, null) { }

        void UpdateTemplate(DataTemplate dataTemplate) {
            mauiViewGroup.RemoveAllViews();

            if (dataTemplate == null)
                return;

            VisualElement templateContent = (VisualElement)VirtualView.ItemTemplate.CreateContent();
            this.VirtualView.Item = templateContent;
            templateContent.PropertyChanged += TemplateContent_PropertyChanged;

            var handler = templateContent.ToHandler(Application.Current.Handler.MauiContext);
            var templatePlatformView = handler.PlatformView ?? handler.ContainerView;

            mauiViewGroup.AddView(templatePlatformView);
            int height = (int)VirtualView.HeightRequest;
            int width = (int)VirtualView.WidthRequest;
            templatePlatformView.Layout(0, 0, width, height);
        }

        void TemplateContent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            Console.WriteLine($"Alert! Property {e.PropertyName} changed!");
        }

        protected override AView CreatePlatformView() {
            return mauiViewGroup = new MauiViewGroup(AApplication.Context);
        }

        protected override void ConnectHandler(AView platformView) {
            base.ConnectHandler(platformView);
            UpdateTemplate(VirtualView.ItemTemplate);
        }

        protected override void DisconnectHandler(AView platformView)
        {
            base.DisconnectHandler(platformView);
        }
	}
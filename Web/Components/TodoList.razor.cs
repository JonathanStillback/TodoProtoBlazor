using System;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using System.Threading.Tasks;
// using System.Collections.Specialized;

namespace Web.Components
{
	public partial class TodoList : ComponentBase
	{
		[Parameter]
		public Web.ViewModels.TodoModel TodoModel {get; set;}
		[Inject]
		IJSRuntime JS {get; set;}

		// public EventCallback<Models.Todo> TodoChanged { get; set; }

		public string hello = "";

		protected override void OnInitialized()
		{
			base.OnInitialized();
		}

		public void CreateTodo()
		{
			Console.WriteLine("Hello from CreateTodo");
			JS.InvokeVoidAsync("console.log", "Hello from Blazor");
			//JS.InvokeVoidAsync("alert", "Hello from from alert");
			hello = "clicked button";
		}

		private void OnTodoChanged(Models.Todo todo)
		{
			Console.WriteLine($"Todo {todo.Name} updated status to {todo.Status}");
		}
		private void UpdateTodo()
		{}

		private void DeleteTodo()
		{}
	}
}
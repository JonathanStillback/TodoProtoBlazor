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
using System.Collections.Generic;
using Models;
using Implementations;
// using System.Collections.Specialized;

namespace Web.Components
{
	public partial class TodoListExample : ComponentBase
	{
		[Parameter]
		public Web.ViewModels.TodoModel TodoModel {get; set;}
		[Inject]
		IJSRuntime JS {get; set;}
		[Inject]
		IProtoClient _protoClient {get; set;}

		[Inject]
		NotifierStateService<Todo> _notifierStateService {get; set;}
		// public EventCallback<Models.Todo> TodoChanged { get; set; }

		public Models.Todo NewTodo {get; set;}
		public string hello = "";

		protected override void OnInitialized()
		{
			base.OnInitialized();
			TodoModel = new Web.ViewModels.TodoModel();
			TodoModel.Todos = new List<Todo>();
			TodoModel.Todos.Add(new Todo() { Name="Test", Description="Cool first task", Status=TodoStatus.New });
			TodoModel.Todos.Add(new Todo() { Name="Second", Description="Cool second task", Status=TodoStatus.Started });
		}

		public void CreateTodo()
		{
			Console.WriteLine("Hello from CreateTodo");
			JS.InvokeVoidAsync("console.log", "Hello from Blazor");
			//JS.InvokeVoidAsync("alert", "Hello from from alert");
			hello = "clicked button";
		}

		private async void OnTodoStatusChanged(ChangeEventArgs e, Todo todo)
		{
			todo.Status = (Models.TodoStatus)Convert.ToInt32(e.Value);
			Console.WriteLine($"Todo {todo.Name} updated status to {todo.Status} (from TodoListExample OnTodoStatus)");
			var todoProto = await _protoClient.DBRequest<Todo>(new DBEntityMessage(todo, DBCommand.Create));
			Console.WriteLine($"Received update of {todo.Name} from Proto - now called {todoProto.Name}");
			todo.Name = todoProto.Name;
			StateHasChanged();
		}

		private void OnTodoChanged(Models.Todo todo)
		{
			Console.WriteLine($"Todo {todo.Name} updated status to {todo.Status} (from TodoListExample OnTodo)");
		}
		private void UpdateTodo()
		{}

		private void DeleteTodo()
		{}
	}
}
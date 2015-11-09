1. Events, Delegates and Lambdas

	a. Events
		Tin can phone => Sender (delegate) => Recipient
			Event raiser => (Delegate) => Event Handler

			Delegate - raising events and sending events
			EventArgs - data that gets sent to Event Handler/Listener

		Role of Events
			Events - are notification (objects). Signal the occurence of an action/notification
			- events pass EventArgs (event data)
			- objects that raise events don't need to explicitly know the object that will handle the event

	b. Delegates
		- Glue/Pipeline between Event and Event Handler
		- specialized class ofthe called a "function pointer"
		- delegate responsible for transfering notificaitons and data (eventArgs) from point A to point B

		Button => click event raised => Delegate => Method handles click event

	c. Event Handlers
		- method that is reponsible for receiving data from Delegate (Inbox)
		- Receives two parameters (Sender(object), EventArgs)
		- EventArgs - responsible for encapsulating event data (Containers)


4. Creating Delegates, Events and EventArgs

	Delegate class
		Method - property, name of the method
		Target - property
		GetInvocationList() - MulticastDelegate -> every delecate inherits from MulticastDelegates (multiple pipelines)

		// Delegate - doesn't return anything
		// When compiler sees word "delegate" it simply creates a class that inherits from MulticastDelegate
		// that accepts hander in the constructor
		// That is why you're new(int) new WorkPerformedHandler below
		public delegate void WorkPerformedHandler(int hours, WorkType workType);

		// Delegate Instance
		// To wire these two together
		// Pass in handler through constructor
		WorkPerformedHandler del1 = new WorkPerformedHandler(WorkPerformed);
		del1(5, WorkType.Golf);

		// To call more handlers
		WorkPerformedHandler del2 = new WorkPerformedHandler(WorkPerformed2);
		del1 += del2

		// Handler - Delegate params and Hanlder params must match up
		public void WorkPerformed(int workHours, WorkType, wType){
			// handle
		}

5. Associating Delegate with Event Raiser
	- events can be defined in a class using event keyword
	- events are friendly wrappers around delegates, so that when EventRaiser is raised, a delegate can route to EventListener
	- Listeners can attach themselves to WorkPerformed event. Behind the scenes listeners subscribe to delegate invocation list of WorkPerformedHandler delegate

	// WorkPerformedHandler is a delegate
	               delegate             event name
	public event WorkPerformedHandler WorkPerformed;

	A. Raising Events
		a. calling event like you would call a method
		if (WorkedPerformed != null) // is anyone attached to event, any listeners?
		{
			// Event is a delegate underneath. So call it like you would a delegate
			WorkPerformed(8, WorkType.GenerateReports);
		}

		b. access even't delegate and invoke it directly, cast event as delegate
		// Event type is a delegate
		WorkPerformedHandler del = WorkPerformed as WorkPerformedHandler;
		if (del != null)
		{
			del(8, WorkType.GenerateReports);
		}
		
6. Creating EventArgs Class
	- should not pass params, but inherit from System.EventArgs

	// EventArgs
	public class WorkPeromedEventArgs : EventArgs
	{
		public int Hours { get; set; }
		public WorkType workType { get; set; }
	}

	- to use custom EventArgs class, delegate must reference the class in its signature
	public delegate void WorkPerformedHandler(object sender, WorkPerformedEventArgs e);

	- using generics to write above different, standard way
	- EventHandler<T> - represents EventArgs

	// compiler defines/generates a delegate 
	// and writes the above delegate signature for us ie. (object sender, EventArgs e)
	// EventHandler<WorkPerformedEventArgs> == delegate void WorkPerformedHandler(object sender, WorkPerformedEventArgs e)
	public event EventHandler<WorkPerformedEventArgs> WorkPerfomed;

7. Event Handlers and Delegates
	- delegate signature must be mimicked by handler method
	- += operator is used to attach an event to event handler

	public void ManagerWorkPerformed(object sender, WorkPerformedEventArgs e) // matched delegate/pipeline signature

	var worker = new Worker();
	// says attach to event through delegate
	// route the data along this delegate/pipeline EventHandler<T> and dump it to worker_WorkPerformed (event handler method)
	worker.WorkPerformed += new EventHandler<WorkPerformedEventArgs>(worker_WorkPerformed);

	Delegate Inference
		- worker.WorkPerformed += worker_WorkPerformed; // no need for EventArgs<T>

8. Anonymous Methods
	- instead of having handler method you have a method on the same line
	- can not attach other events

	worker.WorkPerformed += // Attach event handler here using Anonymous method
	worker.WorkPerformed += delegate(object sender, EventArgs e) {
		// do work here
	};

9. Lambdas
	- clean up code
	- lambda expression can be assigned to any delegate

	//          (s,e) inline method parameters, '=>' is lambda operator
	SubmitButton.Click += (s, e) // params => // lambda operator MessageBox.Show("Button clicked"); // method body

	delegate int AddDelegate(int a, int b);

	a. Parameters
		delegate int AddNumbers(int a, int b);
        AddDelegate del = (a, b) => a + b;
        int result = del(1, 1);

	b. No Parameters
		delegate bool LogDelegate();
		LogDelegate del = () => {
			UpdateDb();
			return true;
		};
		bool status = del;

10. Action<T> 
	- same as: delegate void LogDelegate(int a);
	- accepts a single or more same type parameters and returns no value (one way pipeline)

	// delegate that has signature that accepts one string parameter
	Action<string> messageTarget; // delegate matches this function below
	void ShowMessage(string message){}

	// Action<T> == these two are the same
	public delegate void DelAction(string message);
	public Action<string> DelAction;

11. Func<T, TResult>
	- accepts a single parameter and returns a value of TResult

	// Func<T, TResult> == these two are the same
	public delegate int DelAction(string message);
	public Func<string, int> DelAction;


12. Linq with Func<T, TResult>
	Linq functions accept Func<T,TResult> 

	// Extension method (this) of IEnumerable, returns true/false if it matches
	public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {


Building Final Project 
	- Mediator as Singleton that notifies listeners

    // Same as writing a method yourself
	// meaning whenever I get notified, take parameters s and e and execute method body 
	// (s, e) => { this.DataContext = e.Job; }; == same as having method DoWork(object s, EventArgs e)
    Mediator.GetInstance().JobChanged += (s, e) => { this.DataContext = e.Job; };

using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
	public Transform wander1forpassenger;
	public Transform wander2forpassenger;
	public Transform lightswitch;
	public Transform lightA;
	public Transform lightB;
	public Transform lightC;
	public Transform lightD;

	public Transform doorInside;
	public Transform doorOutside;

	public Transform fishingPoint;
	public Transform poolCenter;
	public Transform gatherPoint;
	public Transform dealPoint;
	public Transform endPoint;

	public GameObject Daniel;
	public GameObject Richard;
	public GameObject Tom;
	public GameObject Passenger;
	public GameObject PassengerB;
	public GameObject door;
	public GameObject ball;

	public Light lightToSwitch1 = null;
	public Light lightToSwitch2 = null;
	public Light lightToSwitch3 = null;
	public Light lightToSwitch4 = null;
	public Light lightToSwitch5 = null;

	public Light lightToSwitchA = null;
	public Light lightToSwitchB = null;
	public Light lightToSwitchC = null;
	public Light lightToSwitchD = null;


	// user interactions
	public bool clickDanial = false;
	public bool clickRichard = false;
	public bool clickTom = false;
	public int stayInHouseTime = 5;

	private BehaviorAgent behaviorAgent;
	private Animator doorAnimator;
	private Animator danielAnimator;
	private bool doorOpen;
	private bool callingDaniel;
	private bool tomIsHere;
	// Use this for initialization
	void Start ()
	{
		tomIsHere = false;
		callingDaniel = false;
		doorOpen = false;
		danielAnimator = Daniel.GetComponent<Animator> ();
		doorAnimator = door.GetComponent<Animator> ();
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		lightToSwitch4.enabled = false;
		lightToSwitch5.enabled = false;
		lightToSwitchA.enabled = true;
		lightToSwitchB.enabled = true;
		lightToSwitchC.enabled = true;
		lightToSwitchD.enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	// TODO: Blackboard data, User Input? Game state?
	private int status = 0;
	private float price = 7;
	private int N;
	private bool danielCall = false;
	private bool tomCall = false;
	private bool danielInHouse = true;
	private bool richardInHouse = false;
	private bool chasing = true;
	private bool gameOver = false;
	private bool pressP = false;
	private bool pressC = false;
	private bool havePolice = false;
	private bool haveConverstation = false;
	public GameObject police;
	public GameObject police1;
	public GameObject police2;
	public GameObject police3;
	public GameObject police4;
	public GameObject police5;

	public Transform wanderp11, wanderp12, wanderp21, wanderp22, wanderp31, wanderp32, wanderp41, wanderp42, wanderp51, wanderp52;

	// new public variables
	public Transform chasePoint1;
	public Transform chasePoint2;
	public Transform chasePoint3;

	// Luren
	public GameObject Luren1, Luren2, Luren3, Luren4, Luren5, Luren6, Luren7, Luren8;
	public Transform LurenMeetingPoint12, LurenMeetingPoint34, LurenMeetingPoint56, LurenMeetingPoint78;
	private Vector3 nextTarget;

	protected Node BuildTreeRoot() {
		//Node node1 = new Sequence (new LeafWait (1000), this.Conversation (Daniel, Tom, gatherPoint), new LeafWait(1000));
		//Node node2 = new Sequence (new LeafWait(1000), this.ExchangeItem(Daniel, Tom, gatherPoint, ball));
		//Node node3 = new Sequence (new LeafWait(1000), this.Follow(Daniel, Tom, Richard));
		//Node node4 = new Sequence (new LeafWait(1000), this.Haggling(Daniel, Tom)); // TODO: when does this event happen? price = 7? or what?

		Node nodeDanielOut = new Sequence (Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("THINK", 1000),
			             this.ST_turnonlight(Daniel),
			             this.ST_GetOutHouse (Daniel),
			             this.ST_SetDanielOut (),
			             this.ST_WalkTo (Daniel, fishingPoint),
			             this.ST_LookAT (Daniel, poolCenter));
		Node nodeLuren = new SequenceParallel (
			                 //this.ST_ThinkUntilClicked (Luren1),
			                 this.Conversation (Luren1, Luren2, LurenMeetingPoint12),
			                 this.Conversation (Luren3, Luren4, LurenMeetingPoint34),
			                 this.Conversation (Luren5, Luren6, LurenMeetingPoint56),
			                 this.Conversation (Luren7, Luren8, LurenMeetingPoint78));
		//Node nodePolice = new Sequence (
		//	                  this.ST_ThinkUntilClicked (police),
		//	                  this.ChaseBoss ());
		Node nodeFish = new DecoratorInvert (
			             new DecoratorLoop (
				             new Sequence (
					             Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Fishing", 1000),
					             new DecoratorInvert (this.ST_Call (Daniel)))));
		Node nodeConverse = this.ConversationNtimes (Daniel, Tom, gatherPoint, 2);
		Node nodeSteal = new Sequence (this.WaitUntilDanielLeave (Richard),
			             this.ST_GetInHouse (Richard),
			             this.ST_SetRichardIn (),
			             Richard.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("ROAR", 1000),
			             this.ST_GrabObj (Richard, ball),
			             this.ST_GetOutHouse (Richard));
		Node nodeChase = this.Chase(Richard, Daniel, Tom, chasePoint1);
		Node nodeTomCall = new Sequence (this.WaitUntilRichardIn (Tom),
			                   this.ST_TomCallDaniel (),
			                   new SequenceParallel (
				                   Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("Talking On Phone", 1000),
				                   Tom.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("Talking On Phone", 1000)));
		Node nodeExchange = new Sequence (new LeafWait (1000), this.ExchangeItem (Richard, PassengerB, dealPoint, ball), this.ST_DropObj(PassengerB, ball));
		//Node nodeEnd = new Sequence (this.ST_BallUp (), this.ST_SetGameOver (), this.ST_CheerN (PassengerB, 5));
		//Node nodeEnd = new Sequence (this.ST_CheerN (PassengerB, 5), new DecoratorForceStatus(RunStatus.Failure, new LeafWait(1000)));
		Node doorNode = this.AssertAndWaitForClap ();
		Node passengerNode = new Sequence (
			                    new DecoratorLoop (
				                    this.ST_Repeat (Passenger)));

		Node nodeMonitorUI = this.MonitorUI ();
		Node nodeMonitorState = this.MonitorState ();
		Node nodeStory1 = this.NewConverse ();
		Node nodeStory2 = this.NewPolice ();
		
		/*Node root1 = new SequenceParallel (
			new Sequence (
				new SequenceParallel (
					new Sequence (nodeDanielOut, nodeFish), 
					nodeSteal,
					nodeTomCall),
				nodeConverse,
				nodeChase,
				nodeExchange,
				this.ST_CheerN(PassengerB, 5)),
			doorNode, passengerNode);
		
		Node nodestory = new DecoratorLoop (
			                 new DecoratorForceStatus (RunStatus.Success,
				                 new Sequence (nodeStory1, nodeStory2)));
		
		Node root = new SequenceParallel (root1,
			            nodeMonitorUI,
			            nodeMonitorState,
			            nodestory);*/
		/*
		Node root = new SequenceParallel (
			new Sequence (
				new SequenceParallel (
					new Sequence (nodeDanielOut, nodeFish), 
					nodeSteal,
					nodeTomCall),
				nodeConverse,
				nodeChase,
				nodeExchange,
				this.ST_CheerN(PassengerB, 5)),
			doorNode, passengerNode, nodeLuren, nodePolice);
		*/
		Node nodePolice1 = new DecoratorLoop (
			new DecoratorForceStatus (RunStatus.Success,
			                 // new SequenceParallel (
				new Sequence (this.ST_Repeatpolice (police1, wanderp11, wanderp12))));
		Node nodePolice2 = new DecoratorLoop (
			new DecoratorForceStatus (RunStatus.Success,
				// new SequenceParallel (
				new Sequence (this.ST_Repeatpolice (police2, wanderp21, wanderp22))));
		/*Node nodePolice3 = new DecoratorLoop (
			new DecoratorForceStatus (RunStatus.Success,
				// new SequenceParallel (
				new Sequence (this.ST_Repeatpolice (police3, wanderp31, wanderp32))));*/
		Node nodePolice4 = new DecoratorLoop (
			new DecoratorForceStatus (RunStatus.Success,
				// new SequenceParallel (
				new Sequence (this.ST_Repeatpolice (police4, wanderp41, wanderp42))));
		Node nodePolice5 = new DecoratorLoop (
			new DecoratorForceStatus (RunStatus.Success,
				// new SequenceParallel (
				new Sequence (this.ST_Repeatpolice (police5, wanderp51, wanderp52))));
				                  //new Sequence (this.ST_Repeatpolice (police2, wanderp21, wanderp22)),
				                  //new Sequence (this.ST_Repeatpolice (police3, wanderp31, wanderp32)),
				                  //new Sequence (this.ST_Repeatpolice (police4, wanderp41, wanderp42)),
					//new Sequence (this.ST_Repeatpolice (police5, wanderp51, wanderp52)))));
		
		Node Danielcontrol = new DecoratorLoop (
			                     new DecoratorForceStatus (RunStatus.Success,
				                     new Sequence (
					                     this.ST_ThinkUntilClickedLight (Daniel),
					                     this.clickLight (Daniel))));
		
		Node root = new SequenceParallel (
			this.gameOverNew(), Danielcontrol, doorNode, passengerNode, nodeLuren, nodePolice1, nodePolice2, nodePolice4, nodePolice5);

		return root;
	}

	protected Node ST_Repeatpolice(GameObject guy, Transform position1, Transform position2) {
		return new Sequence (this.ST_ApproachAndWait (guy, position1), this.ST_ApproachAndWait (guy, position2));
	}

	protected Node gameOverNew() {
		Func<bool> act1 = () => notNearEnough (police1, Daniel);
		Func<bool> act2 = () => notNearEnough (police2, Daniel);
		Func<bool> act3 = () => notNearEnough (police3, Daniel);
		Func<bool> act4 = () => notNearEnough (police4, Daniel);
		Func<bool> act5 = () => notNearEnough (police5, Daniel);

		return new DecoratorLoop (
			new SequenceParallel (
				new LeafAssert (act1), new LeafAssert (act2), new LeafAssert (act3), new LeafAssert (act4), new LeafAssert (act5)));
	}

	private bool notNearEnough(GameObject guy1, GameObject guy2) {
		if ((guy1.GetComponent<Transform>().position - guy2.GetComponent<Transform>().position).magnitude < 2)
			return false;
		return true;
	}
		
	protected Node ST_turnonlight(GameObject guy, Light light) {
		return new Sequence (
			guy.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("ReachRight", 1000), this.Turnlightnew (guy, light));
	}

	protected Node Turnlightnew(GameObject guy, Light light)
	{
		return new LeafInvoke (() => turnlight (guy, light));
	}

	protected Node clickLight(GameObject guy) {
		return new SelectorParallel (
			new Sequence (
				new LeafInvoke (() => click1 ()),
				this.ST_WalkTo (Daniel, lightA),
				this.ST_turnonlight (Daniel,
					lightToSwitchA)),
			new Sequence (
				new LeafInvoke (() => click2 ()),
				this.ST_WalkTo (Daniel, lightB),
				this.ST_turnonlight (Daniel,
					lightToSwitchB)),
			new Sequence (
				new LeafInvoke (() => click3 ()),
				this.ST_WalkTo (Daniel, lightC),
				this.ST_turnonlight (Daniel,
					lightToSwitchC)),
			new Sequence (
				new LeafInvoke (() => click4 ()),
				this.ST_WalkTo (Daniel, lightD),
				this.ST_turnonlight (Daniel,
					lightToSwitchD)),
			new Sequence (
				new LeafInvoke (() => click5 ()),
				this.ST_GetInHouse (Daniel)));
	}

	public void InNextLevel() {
		Application.LoadLevel (2);
	}
	public void UpperLevel() {
		Application.LoadLevel (0);
	}
	/*protected Node GetInNewScene() {
		Application.LoadLevel (2);
		return new LeafWait (1000);
	}*/
			
	RunStatus click1 () {
		Val<bool> click = Val.V (() => Input.GetKey (KeyCode.Z));
		if (click.Value)
			return RunStatus.Success;
		return RunStatus.Failure;
	}
	RunStatus click2 () {
		Val<bool> click = Val.V (() => Input.GetKey (KeyCode.X));
		if (click.Value)
			return RunStatus.Success;
		return RunStatus.Failure;
	}
	RunStatus click3 () {
		Val<bool> click = Val.V (() => Input.GetKey (KeyCode.C));
		if (click.Value)
			return RunStatus.Success;
		return RunStatus.Failure;
	}
	RunStatus click4 () {
		Val<bool> click = Val.V (() => Input.GetKey (KeyCode.V));
		if (click.Value)
			return RunStatus.Success;
		return RunStatus.Failure;
	}
	RunStatus click5 () {
		Val<bool> click = Val.V (() => Input.GetKey (KeyCode.I));
		if (click.Value)
			return RunStatus.Success;
		return RunStatus.Failure;
	}
		
	protected Node ChaseBoss() {
		return new Sequence (
			this.ST_WalkToConverse (police, PassengerB.transform),
			new DecoratorForceStatus (RunStatus.Failure, new LeafWait (1000)));
	}

	protected Node NewPolice() {
		return new SelectorParallel (
			new DecoratorLoop (
				this.IsPPressed ()),
			this.ChaseRichard ());
	}
	protected Node IsPPressed() {
		Func<bool> act = ()=>pressP == true;
		return new LeafAssert(act);
	}
	protected Node ChaseRichard() {
		return new LeafWait (1000);// TODO
	}


	protected Node NewConverse() {
		return new SelectorParallel (
			new DecoratorInvert (
				new DecoratorLoop (
					this.IsCPressed ())),
			this.Conversation (Luren1, Luren2, LurenMeetingPoint12));
	}



	protected Node IsCPressed() {
		Func<bool> act = () => pressC == true;
		return new LeafAssert (act);
	}

	protected Node MonitorState() {
		return new DecoratorLoop (
			new DecoratorForceStatus (RunStatus.Success, 
				new Selector (
					this.CheckPolice (),
					this.CheckConversation (),
					new LeafWait (1000))));
	}

	protected Node CheckConversation() {
		Func<bool> check1 = () => pressC == true;
		return new Sequence (new LeafAssert (check1), this.SetConversation ());
	}
	protected Node SetConversation() {
		Func<bool> act = () => setConversation ();
		return new LeafAssert(act);
	}
	private bool setConversation() {
		haveConverstation = true;
		return true;
	}

	protected Node CheckPolice() {
		Func<bool> check1 = () => pressP == true;
		return new Sequence (new LeafAssert(check1), this.SetPolice());
	}
	protected Node SetPolice() {
		Func<bool> act = () => setPolice ();
		return new LeafAssert(act);
	}
	private bool setPolice() {
		havePolice = true; // TODO: set pressP back?
		return true;
	}

	protected Node MonitorUI() {
		return new DecoratorLoop (
			new DecoratorForceStatus (RunStatus.Success,
				new Sequence (
					this.MonitorP (),
					this.MonitorC ())));
	}

	protected Node MonitorP() {
		Func<bool> act = () => MonitorPressP ();
		return new LeafAssert (act);
	}
	private bool MonitorPressP() {
		if (Input.GetKeyDown (KeyCode.P)) {
			pressP = true;
		}
		return true;
	}
	protected Node MonitorC() {
		Func<bool> act = () => MonitorPressC ();
		return new LeafAssert (act);
	}
	private bool MonitorPressC() {
		if (Input.GetKeyDown (KeyCode.C)) {
			pressC = true;
		}
		return true;
	}

	protected Node ST_CheerN(GameObject guy, int n) {
		return new Sequence (
			this.ST_SetN (n),
			new DecoratorInvert (
				new DecoratorLoop (
					new Sequence (this.ST_Ntimes(),
						this.ST_LookAT (guy, ball.transform),
						guy.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("CHEER", 1000),
						this.ST_ReduceN ()))));
	}

	protected Node ST_SetGameOver() {
		Func<bool> act = () => SetGameOver ();
		return new LeafAssert (act);
	}

	private bool SetGameOver() {
		gameOver = true;
		return true;
	}

	//protected Node ST_BallUp() {
	//	Func<bool> act = () => BallUp ();
	//	return new LeafAssert(act);
	//}

	//private bool BallUp() {
	//	ball.transform.position.y = 3;
	//	return true;
	//}

	protected Node ST_Repeat(GameObject guy)
	{
		return new Sequence (this.ST_ApproachAndWait (guy, this.wander2forpassenger), Passenger.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("ReachRight", 1000),
			this.Turnlight (guy), this.ST_ApproachAndWait (guy, this.wander1forpassenger), Passenger.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("ReachRight", 1000),
			this.Turnlight (guy));
	
	}

	protected Node ST_turnonlight(GameObject guy)
	{
		return new Sequence (
			this.ST_WalkTo (guy, lightswitch), guy.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("ReachRight", 1000), this.Turnlight (guy));
	}

	protected Node Turnlight(GameObject guy)
	{
		if(guy.tag!="Passenger") return new LeafInvoke(()=>turnlightpp(guy));
			else return new LeafInvoke(()=>turnlightrepeat());
	}
	RunStatus turnlightpp(GameObject guy)
	{   
		return RunStatus.Failure;
	}

			RunStatus turnlightrepeat()
			{
		if (lightToSwitch4.enabled == true) {
			lightToSwitch4.enabled = false;
			lightToSwitch5.enabled = true;
		} else {
			lightToSwitch4.enabled = true;
			lightToSwitch5.enabled = false;
		}
		return RunStatus.Success;
			}

	RunStatus turnlight(GameObject guy, Light light)
	{   if (guy.tag == "Daniel") {
			if (light.enabled == true) {
				light.enabled = false;
			} else {
				light.enabled = true;

			}

			return RunStatus.Success;
		
		} else
			return RunStatus.Failure;
			}

	protected Node ST_TomCallDaniel() {
		Func<bool> act = () => TomCallDaniel ();
		return new LeafAssert(act);
	}

	private bool TomCallDaniel() {
		danielCall = true;
		tomCall = true;
		return true;
	}

	protected Node WaitUntilRichardIn(GameObject guy) {
		Func<bool> act = () => RichardOutHouse ();
		return new DecoratorInvert (
			new DecoratorLoop (
				new LeafAssert (act)));
	}

	private bool RichardOutHouse() {
		if (richardInHouse)
			return false;
		return true;
	}

	protected Node ST_SetRichardIn() {
		Func<bool> act = () => RichardInHouse ();
		return new LeafAssert(act);
	}

	private bool RichardInHouse() {
		richardInHouse = true;
		return true;
	}

	protected Node ST_SetDanielOut() {
		Func<bool> act = () => DanielOutHouse ();
		return new LeafAssert(act);
	}

	private bool DanielOutHouse() {
		danielInHouse = false;
		return true;
	}

	protected Node WaitUntilDanielLeave(GameObject guy) {
		Func<bool> act = () => DanielInHouse ();
		return new DecoratorInvert (
			new DecoratorLoop (
				new LeafAssert (act)));
	}

	private bool DanielInHouse() {
		if (danielInHouse)
			return true;
		return false;
	}

	protected Node ST_Call(GameObject guy) {
		Func<bool> act = () => checkCall (guy);
		return new LeafAssert(act);
	}

	private bool checkCall(GameObject guy) {
		if (guy.tag == "Daniel") {
			if (danielCall)
				return true;
		}
		else if (guy.tag == "Tom") {
			if (tomCall)
				return true;
		}
		return false;
	}

	// TODO:
	protected Node Haggling(GameObject buyer, GameObject seller) {
		Func<bool> act = () => price > 4;
		return new Sequence (this.ST_WalkToConverse (buyer, gatherPoint),
			this.ST_WalkToConverse (seller, gatherPoint),
			new DecoratorInvert (
				new DecoratorLoop (
					new Sequence (new LeafAssert (act), this.ST_Haggle (buyer), this.ST_RandomSuccess (0.8f), this.ST_Agree (seller), this.ST_LowerPrice ()))),
			new Selector (
				new LeafAssert (act), new Sequence (this.ST_GrabObj (buyer, ball), this.ST_WalkToConverse (buyer, gatherPoint))));
	}

	protected Node ST_Haggle(GameObject guy) {
		return guy.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("HANDSUP", 1000);
	}

	protected Node ST_RandomSuccess(float thresh) {
		Func<bool> act = () => UnityEngine.Random.Range (0.0f, 1.0f) < thresh;
		return new LeafAssert (act);
	}

	protected Node ST_Agree(GameObject guy) {
		return guy.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 1000);
	}

	private bool ReducePrice() {
		price = price - 1.0f;
		return true;
	}

	protected Node ST_LowerPrice() {
		Func<bool> act = () => ReducePrice();
		//Func<bool> act = () => price = price - 1.0f;
		return new LeafAssert(act);
	}

	protected Node Follow(GameObject guy1, GameObject guy2, GameObject guy3) {
		// TODO: maybe here we could use the blackbord to know the destination, now assume the destination is gatherPoint	
		return new Sequence (this.ST_WalkToConverse (guy1, gatherPoint),
			new SequenceParallel (this.ST_WalkToConverse (guy2, guy1.transform),
				this.ST_WalkToConverse (guy3, guy1.transform)));
	}

	protected Node Chase(GameObject guy1, GameObject guy2, GameObject guy3, Transform target) {
		return new DecoratorInvert (
			new DecoratorLoop (
				new SequenceParallel (
					new DecoratorLoop (
						this.ST_IsChasing ()),
					new DecoratorLoop (
						new Sequence (
							this.ST_WalkTo (guy1, chasePoint1),
							this.ST_WalkTo (guy1, chasePoint2),
							this.ST_WalkTo (guy1, chasePoint3),
							this.ST_ChangeChasing ())),
					new DecoratorLoop (
						this.ST_WalkToConverse (guy2, guy1.transform)),
					new DecoratorLoop (
						this.ST_WalkToConverse (guy3, guy1.transform)))));
	}
	private int numChase = 1; // TTTT
	protected Node ST_ChangeChasing() {
		Func<bool> act = () => ChangeChasing ();
		return new LeafAssert(act);
	}
	private bool ChangeChasing() {
		if (numChase <= 0)
			chasing = false;
		numChase = numChase - 1;
		return true;
	}

	protected Node ST_IsChasing() {
		Func<bool> act = () => IsChasing ();
		return new LeafAssert (act);
	}

	private bool IsChasing() {
		if (chasing)
			return true;
		return false;
	}

	protected Node ExchangeItem(GameObject guy1, GameObject guy2, Transform point, GameObject obj) {
		return new Sequence (this.ST_GrabObj (guy1, obj),
			new SequenceParallel (this.ST_WalkToConverse (guy1, point), this.ST_WalkToConverse (guy2, point)),
			new Sequence (
				new LeafWait(1000), new LeafWait(1000), new DecoratorForceStatus(RunStatus.Success, this.ST_DropObj(guy1, obj)), this.ST_GrabObj(guy2, obj), this.ST_WalkTo(guy2, endPoint)));
	}

	protected Node ST_DropObj(GameObject guy, GameObject obj) {
		Func<bool> act = () => IKDrop (guy);
		return new LeafAssert (act);
	}

	private bool IKDrop(GameObject guy) {
		ball.transform.parent = null;
		guy.GetComponent<_IKController>().ikActive = false;
		return true;
	}

	// how to grab a ball object
	protected Node ST_GrabObj(GameObject guy, GameObject obj) {
		//Debug.Log (guy1.transform.Find ("TARGET_GivePoint").position);
		//Debug.Log(guy.transform.Find("Daniel").transform.Find("UMA_Male_Rig"));
		Debug.Log ("ST_GrabObj is called");
		//return new Sequence (this.ST_WalkTo (guy, obj.transform.Find ("FrontPoint")),
		//	this.ST_HelpGrabObj (guy, obj));
		return new Sequence (this.ST_WalkToConverse2 (guy, obj.transform, 0.8f),
			this.ST_HelpGrabObj (guy, obj));
	}

	protected Node ST_HelpGrabObj(GameObject guy, GameObject obj) {

		// need to reach out the right hand first? TODO
		// may use Inverse Kinematic
		Func<bool> ikgrab = () => IKGrab(guy);
		//Func<bool> ikgrab2 = () => IKGrab2 (guy);
		Func<bool> act = () => (obj.transform.parent = guy.transform.Find("TARGET_TakePoint").transform.Find("RightHand").transform);
		//Func<bool> act = () => (obj.transform.parent = guy.transform.Find("Interactions").transform.Find("INTER_HoldRight").transform);
		//return new DecoratorForceStatus(RunStatus.Success, new Sequence (new LeafAssert(ikgrab), new LeafAssert (act), new LeafAssert(ikgrab2)));
		return new Sequence (new LeafAssert(ikgrab), new LeafAssert (act)); //new LeafAssert(ikgrab2));
	}

	private bool IKGrab(GameObject guy) {
		guy.GetComponent<_IKController>().ikActive = true;
		return true;
	}

	private bool IKGrab2(GameObject guy) {
		guy.GetComponent<_IKController> ().ikActive = false;
		return true;
	}

	// parameterized behavior tree: a conversation
	protected Node Conversation(GameObject guy1, GameObject guy2, Transform meetingPoint) {
		return new Sequence (new SequenceParallel (new Sequence (this.ST_WalkToConverse (guy1, meetingPoint),
			this.ST_GazeAt (guy1, guy2)), new Sequence (this.ST_WalkToConverse (guy2, meetingPoint),
			this.ST_GazeAt (guy2, guy1))),
			new DecoratorInvert (
				new DecoratorLoop (
					new Sequence (this.ST_GazeAt(guy1, guy2),
						this.ST_GazeAt(guy2, guy1),
						guy1.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("WAVE", 1000),
						guy2.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("WAVE", 1000), new LeafWait (1000)))));
	}

	protected Node ConversationNtimes(GameObject guy1, GameObject guy2, Transform meetingPoint, int n) {
		return new Sequence (new SequenceParallel (new Sequence (this.ST_WalkToConverse (guy1, meetingPoint),
			this.ST_GazeAt (guy1, guy2)), new Sequence (this.ST_WalkToConverse (guy2, meetingPoint),
			this.ST_GazeAt (guy2, guy1))),
			this.ST_SetN (n),
			new DecoratorInvert (
				new DecoratorLoop (
					new Sequence (this.ST_Ntimes (),
						this.ST_GazeAt(guy1, guy2),
						this.ST_GazeAt(guy2, guy1),
						guy1.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("WAVE", 1000),
						guy2.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("WAVE", 1000),
						new LeafWait (500),
						this.ST_ReduceN ()))));
	}

	protected Node ST_WalkToConverse(GameObject guy, Transform meetingPoint) {
		Val<Vector3> position = Val.V(() => (meetingPoint.position + Vector3.Normalize((guy.GetComponent<Transform>().position - meetingPoint.position))));
		return guy.GetComponent<BehaviorMecanim>().Node_GoTo(position);
	}

	protected Node ST_WalkToConverse2(GameObject guy, Transform meetingPoint, float tt) {
		Val<Vector3> position = Val.V(() => (meetingPoint.position + tt * Vector3.Normalize((guy.GetComponent<Transform>().position - meetingPoint.position))));
		return guy.GetComponent<BehaviorMecanim>().Node_GoTo(position);
	}

	// TODO: how to Gaze At the head?
	protected Node ST_GazeAt(GameObject guy1, GameObject guy2) {
		return this.ST_LookAT (guy1, guy2.transform.Find("Daniel").transform.Find("UMA_Male_Rig").transform);
	}
	// TODO: how to play random Gesture?
		
	/*protected Node BuildTreeRoot()
	{
		Node danielNode = new Sequence (
			                  this.ST_ThinkUntilClicked (Daniel),
			                  this.ST_GetOutHouse (Daniel),
			                  this.ST_WalkTo (Daniel, fishingPoint),
			                  this.ST_LookAT (Daniel, poolCenter),
			                  new DecoratorInvert (
				                  new DecoratorLoop (
					                  new Sequence (
						                  Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("FISHING", 1000),
						                  new DecoratorInvert (this.IsClicked (Tom))))),
			                  Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("TALKING ON PHONE", 1000),
			                  //this.ST_WalkTo (Daniel, gatherPoint),
			                  this.ST_SetN (2),
			                  new DecoratorInvert (
				                  new DecoratorLoop (
					                  new Sequence (
						                  this.ST_Ntimes (), this.Conversation (Daniel, Tom, gatherPoint), this.ST_ReduceN ()))),
			                  this.ST_GetInHouse (Daniel),
			                  //this.ST_WalkTo (barPoint),
			                  //this.ST_GrabBar (Daniel),
			                  new DecoratorLoop (
				                  Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("FIGHT", 1000)));
		
		Node richardNode = new Sequence (
			                   this.ST_ThinkUntilClicked (Richard),
			                   this.ST_GetInHouse (Richard),
			                   Richard.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("ROAR", 1000),
			                   new LeafWait (1000),
			                   Richard.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("ROAR", 1000),
			                   new DecoratorInvert (
				                   this.ST_Wander (Richard)),
			                   new DecoratorInvert (
				                   new Sequence (
					                   this.WaitForFight (),
					                   Richard.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("SURRENDER", 1000),
					new DecoratorForceStatus (RunStatus.Success, new LeafWait (1000)))),//!!!!!!!!!!!!!!!!!
			                   this.ST_GetOutHouse (Richard),
			                   new DecoratorForceStatus (RunStatus.Failure, new LeafWait (1000)));

		Node tomNode = new Sequence (
			               this.AssertAndWaitForRoar (),
			               this.ST_HeadShakeThinkUntilClicked (Tom),
			               this.ST_CallDaniel (),
			               //this.ST_WalkTo (Tom, gatherPoint),
			               //new LeafWait (8000),
			               this.ST_WalkTo (Tom, doorInside),
			               new DecoratorLoop (
				               Tom.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("FIGHT", 1000)));

		Node doorNode = this.AssertAndWaitForClap ();

		Node root = new SequenceParallel(danielNode, richardNode, tomNode, doorNode);
		return root;
	}*/

	protected Node ST_SetN(int n) {
		Func<bool> act = () => SetN (n);
		return new LeafAssert(act);
	}

	private bool SetN(int n) {
		N = n;
		return true;
	}

	protected Node ST_Ntimes() {
		Func<bool> act = () => Ntimes ();
		return new LeafAssert (act);
	}

	private bool Ntimes() {
		if (N <= 0)
			return false;
		else
			return true;
	}

	private Node ST_ReduceN() {
		Func<bool> act = () => ReduceN ();
		return new LeafAssert (act);
	}

	private bool ReduceN() {
		N = N - 1;
		return true;
	}

	protected Node ST_Wander(GameObject guy) {
		Node roaming = new DecoratorLoop (
			               new Sequence (
				               this.ST_ApproachAndWait (guy, this.wander1),
				               this.ST_ApproachAndWait (guy, this.wander2),
				               this.ST_ApproachAndWait (guy, this.wander3),
				               new LeafInvoke (() => TimeLeft ())));
				
		return roaming;
	}

	protected Node AssertAndWaitForFight() {
		return new Sequence (
			new DecoratorInvert (
				new DecoratorLoop (
					new DecoratorInvert (
						new Sequence (this.WaitForFight ())))),
			new LeafWait (1000));
	}

	protected Node WaitForFight() {
		return new LeafAssert (() => ((Tom.GetComponent<Animator> ().GetBool ("B_Idle_Fight")) || (Daniel.GetComponent<Animator>().GetBool("B_Idle_Fight"))));
	}

	protected Node ST_LookAT(GameObject guy, Transform target) {
		Val<Vector3> position = Val.V (() => target.position);
		return guy.GetComponent<BehaviorMecanim> ().Node_OrientTowards (position);
	}

	protected Node TomIsHere() {
		Func<bool> tomhere = () => (tomIsHere == false);
		return new LeafAssert (tomhere);
	}

	protected Node SetTomIsHere() {
		tomIsHere = true;
		return new LeafWait (1000);
	}

	protected Node ST_CallDaniel() {
		callingDaniel = true;
		return Tom.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("TALKING on PHONE", 1000);
	}

	protected Node ST_PhoneCall() {
		Func<bool> makeCall = () => (callingDaniel == false);
		return new LeafAssert (makeCall);
	}
//	protected Node ST_PhoneCall(GameObject guy) {
//		return new LeafInvoke (() => ReturnSuccess());
//	}

	protected Node AssertAndWaitForRoar() {
		return new Sequence (
				new DecoratorInvert (
					new DecoratorLoop (
						new DecoratorInvert (
							new Sequence (this.WaitForRoar ())))),
				new LeafWait (1000));
	}

	protected Node WaitForRoar() {
		return new LeafAssert (() => Richard.GetComponent<Animator> ().GetBool ("F_Roar"));
	}

	protected Node ST_ThinkUntilClicked(GameObject guy) {
		return new DecoratorInvert (
			new DecoratorLoop (
				new Sequence (
					guy.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("THINK", 1000),
					new DecoratorInvert (this.IsClicked (guy)))));
	}

	protected Node ST_ThinkUntilClickedLight(GameObject guy) {
		return new DecoratorInvert (
			new DecoratorLoop (
				new Sequence (
					guy.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("THINK", 1000),
					new DecoratorInvert (this.clickLight (guy)))));
	}

	protected Node ST_HeadShakeThinkUntilClicked(GameObject guy) {
		return new DecoratorInvert (
			new DecoratorLoop (
				new Sequence (
					guy.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("HEADSHAKETHINK", 1000),
					new DecoratorInvert (this.IsClicked (guy)))));
	}

	RunStatus TimeLeft() {
		if (stayInHouseTime == 0)
			return RunStatus.Failure;
		stayInHouseTime--;
		return RunStatus.Success;
	}

	protected Node ST_GetOutHouse(GameObject guy) {
		return new Sequence (
			this.ST_WalkTo (guy, doorInside), guy.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CLAP", 1000), this.ST_WalkTo (guy, doorOutside), guy.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CLAP", 1000));
	}

	protected Node ST_GetInHouse(GameObject guy) {
		return new Sequence (
			this.ST_WalkTo (guy, doorOutside), guy.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CLAP", 1000), this.ST_WalkTo (guy, doorInside), guy.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CLAP", 1000));
	}
		
	RunStatus ReturnSuccess() {
		return RunStatus.Success;
	}
		
	/*
	protected Node ST_OpenDoor(GameObject guy) {
		return new LeafInvoke (() => ReturnSuccess());
	}
	protected Node ST_CloseDoor(GameObject guy) {
		return new LeafInvoke (() => ReturnSuccess());
	}

	protected Node ST_WaitForGather(GameObject guy) {
		return new LeafInvoke (() => ReturnSuccess());
	}
	protected Node ST_GrabBar(GameObject guy) {
		return new LeafInvoke (() => ReturnSuccess());
	}
*/
	protected Node ST_ApproachAndWait(GameObject guy, Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( new DecoratorInvert (this.WaitForFight()), guy.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node ST_WalkTo(GameObject guy, Transform target) {
		Val<Vector3> position = Val.V (() => target.position);
		return guy.GetComponent<BehaviorMecanim> ().Node_GoTo (position);
	}

	protected Node IsClicked(GameObject guy) {
		return new LeafInvoke (() => isclicked(guy));
	}

	RunStatus isclicked(GameObject guy) {
		Val<bool> click;
		if (guy.tag == "Luren")
			click = Val.V (() => Input.GetKey(KeyCode.C));
		else if (guy.tag == "Police")
			click = Val.V (() => Input.GetKey(KeyCode.P));
		else if (guy.tag == "Tom")
			click = Val.V (() => clickTom);
		else
			return RunStatus.Failure;
		
		if (click.Value)
			return RunStatus.Success;
		return RunStatus.Failure;
	}

/*	protected Node AssertAndWaitForClap() {
		return new DecoratorLoop () (
			new Sequence (
				new DecoratorInvert (
					new DecoratorLoop ((new DecoratorInvert (
						new Sequence (this.WaitForClap ()))))),
				new LeafWait (1000), OpenDoor ()));
	}
*/
/*	protected Node OpenDoor() {
		Vector3 doorPosition = door.transform.position;
		doorPosition.y = doorPosition.y + 5;
		return new Sequence (new LeafInvoke (() => StartCoroutine (MoveObject (door.transform, doorPosition, 4f))), new LeafWait (1000));
	}
*/

	RunStatus DoorControl() {
		if (doorOpen == false) {
			doorOpen = true;
			doorAnimator.SetTrigger ("Open");
		} else if (doorOpen == true) {
			doorOpen = false;
			doorAnimator.SetTrigger ("Close");
		}

		return RunStatus.Success;
	}
	
	protected Node OpenDoor() {
		return new LeafInvoke (() => DoorControl ());
	}
	
	protected Node AssertAndWaitForClap() {
		return new DecoratorLoop (
			new Sequence (
				new DecoratorInvert (
					new DecoratorLoop (
						new DecoratorInvert (
							new Sequence (this.DoorWaitForClap ())))),
				new LeafWait (1000), OpenDoor ()));
	}

	protected Node DoorWaitForClap() {
	return new LeafAssert (() => ((Daniel.GetComponent<Animator> ().GetBool ("H_Clap")) || (Richard.GetComponent<Animator>().GetBool("H_Clap"))));
	}
}
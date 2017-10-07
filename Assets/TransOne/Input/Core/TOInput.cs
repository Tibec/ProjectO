using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Sony controller inputs
/// </summary>
public enum PSInputs
{
    Cross, Circle, Square, Triangle,
    Start, Select,
    L1, L2, L3,
    R1, R2, R3,
    LeftAxisX, LeftAxisY, RightAxisX, RightAxisY,
	Up, Down, Left, Right,DPad
}

/// <summary>
/// Microsoft controller inputs
/// </summary>
public enum XboxInputs
{
    A, B, X, Y,
    Start, Select,
    /*LT,*/ LB, L3,
    /*RT,*/ RB, R3,
	Trigger,
    LeftAxisX, LeftAxisY, RightAxisX, RightAxisY,
	Up, Down, Left, Right,DPad
}

/// <summary>
/// Valve controller inputs
/// </summary>
public enum ValveInputs
{
    Trigger,Steam,Menu,Grip,PadTouch,PadPress,PadX,PadY
}




/// <summary>
/// Wii controller inputs.
/// </summary>
public enum WiiInputs
{
    A, B, C, Z, One, Two,
    Plus, Minus,Home,
    LeftAxisX, LeftAxisY, RightAxisX, RightAxisY,
	Up, Down, Left, Right,DPad
}

public enum TOAxis
{
    AxisX1,AxisY1, AxisX2, AxisY2, AxisX3, AxisY3,
    AxisX4, AxisY4, AxisX5, AxisY5, AxisX6, AxisY6,
}

public enum TOButtons
{
    Button1, Button2, Button3, Button4, Button5,
    Button6, Button7, Button8, Button9, Button10,
    Button11, Button12, Button13, Button14, Button15,
	Button16, Button17, Button18, Button19, Button20

}
public enum TOTrac
{
    Position,Rotation

}


/// <summary>
/// Key code used by VRPN & windows,see https://msdn.microsoft.com/fr-fr/library/windows/desktop/dd375731(v=vs.85).aspx for more details
/// </summary>
public enum KeyCodeVRPN
{
    LMouse = 1,
    RMouse = 2,
    CMouse = 4,
    Back = 8,
    Tab = 9,
    Clear = 12,
    Enter = 13,
    Shift = 16,
    Ctrl = 17,
    Alt = 18,
    Pause = 19,
    Esc = 27,
    Space = 32,
    PgUp = 33,
    PgDown = 34,
    End = 35,
    Home = 36,
    LeftArrow = 37,
    UpArrow = 38,
    RightArrow = 39,
    DownArrow = 40,
    Select = 41,
    Print = 42,
    Exec = 43,
    PrintScr = 44,
    Ins = 45,
    Del = 46,
    Help = 47,
    Alpha0 = 48,
    Alpha1 = 49,
    Alpha2 = 50,
    Alpha3 = 51,
    Alpha4 = 52,
    Alpha5 = 53,
    Alpha6 = 54,
    Alpha7 = 55,
    Alpha8 = 56,
    Alpha9 = 57,
    A = 65,
    B = 66,
    C = 67,
    D = 68,
    E = 69,
    F = 70,
    G = 71,
    H = 72,
    I = 73,
    J = 74,
    K = 75,
    L = 76,
    M = 77,
    N = 78,
    O = 79,
    P = 80,
    Q = 81,
    R = 82,
    S = 83,
    T = 84,
    U = 85,
    V = 86,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,
    WinL = 91,
    WinR = 92,
    Numpad0 = 96,
    Numpad1 = 97,
    Numpad2 = 98,
    Numpad3 = 99,
    Numpad4 = 100,
    Numpad5 = 101,
    Numpad6 = 102,
    Numpad7 = 103,
    Numpad8 = 104,
    Numpad9 = 105,
    Mult =106,
    Add = 107,
    Sep = 108,
    Sub = 109,
    Dec = 110,
    Div = 111,
    F1 = 112,
    F2 = 113,
    F3 = 114,
    F4 = 115,
    F5 = 116,
    F6 = 117,
    F7 = 118,
    F8 = 119,
    F9 = 120,
    F10 = 121,
    F11 = 122,
    F12 = 123,
    F13 = 124,
    F14 = 125,
    F15 = 126,
    F16 = 127
 
}
/// <summary>
/// Abstract class to use a device on unity
/// its the high level class (no access to the driver)
/// </summary>
public abstract class TOInput 
{
	/// <summary>
	/// All inputs for the device
	/// </summary>
    protected Dictionary<IConvertible, BasicInputTO> inputs;
	/// <summary>
	/// The full IP address and name.
	/// </summary>
    protected string fullAddress="";
	/// <summary>
	/// All instances of the device in unity(eg. for two controllers we will have two instances)	
	/// </summary>
	public static Dictionary<int, TOInput> instances = new Dictionary<int, TOInput>();



	public TOInput(string name, string address)
	{
		if (address != "")
			fullAddress = name + "@" + address;
		else
			fullAddress = name;

		inputs = new Dictionary<IConvertible, BasicInputTO>();

	}
		

	/// <summary>
	/// Gets the input from an name
	/// </summary>
	/// <returns>The input.</returns>
	/// <param name="nameButton">Name of button</param>
	/// <param name="id">Instance id</param>
    private static BasicInputTO GetInput(IConvertible nameButton, int id)
    {
        BasicInputTO b;
        instances[id].inputs.TryGetValue(nameButton, out b);
        return b;
    }
		
	protected static T CreateInput<T>(int id,BasicInputTO.typeInput type){

		return (T)Activator.CreateInstance (typeof(T), new object[]{id,type});
	}

	protected static T CreateInput<T>(int id,BasicInputTO.typeInput type,int idSub){

		return (T)Activator.CreateInstance (typeof(T), new object[]{id,type,idSub});
	}

    protected static bool A_GetButtonDown(IConvertible nameButton, int id)
    {
		return GetInput(nameButton, id).GetButtonDown(instances[id].fullAddress,nameButton);
    }

    protected static bool A_GetButton(IConvertible nameButton, int id)
    {
		return GetInput(nameButton, id).GetButton(instances[id].fullAddress,nameButton);
    }

    protected static bool A_GetButtonUp(IConvertible nameButton, int id)
    {	
		return GetInput(nameButton, id).GetButtonUp(instances[id].fullAddress,nameButton);
    }

    protected static float A_GetAxis(IConvertible nameButton, int id)
    {
		return GetInput(nameButton, id).GetAxis(instances[id].fullAddress,nameButton);
    }

    protected static Vector3 A_GetPosition(IConvertible nameButton, int id)
    {
		return GetInput(nameButton, id).GetPosition(instances[id].fullAddress);
    }

    protected static Quaternion A_GetRotation(IConvertible nameButton, int id)
    {
		return GetInput(nameButton, id).GetRotation(instances[id].fullAddress);
    }

    public static List<BasicInputTO> returnInstance(int id)
    {
        return instances[id].inputs.Values.ToList();
    }



}

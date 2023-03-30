
using UnityEngine;
using System.Threading;
using System.Collections;
using System.IO.Ports;

/**
 * This class allows a Unity program to continually check for messages from a
 * serial device.
 *
 * It creates a Thread that communicates with the serial port and continually
 * polls the messages on the wire.
 * That Thread puts all the messages inside a Queue, and this SerialController
 * class polls that queue by means of invoking SerialThread.GetSerialMessage().
 *
 * The serial device must send its messages separated by a newline character.
 * Neither the SerialController nor the SerialThread perform any validation
 * on the integrity of the message. It's up to the one that makes sense of the
 * data.
 */
public class SerialController : MonoBehaviour
{
    [Tooltip("Port name with which the SerialPort object will be created.")]
    public string portName = "COM10";

    [Tooltip("Baud rate that the serial device is using to transmit data.")]
    public int baudRate = 9600;

    // [Tooltip("Reference to an scene object that will receive the events of connection, " +
    //          "disconnection and the messages from the serial device.")]
    // public GameObject messageListener;

    // [Tooltip("After an error in the serial communication, or an unsuccessful " +
    //          "connect, how many milliseconds we should wait.")]
    // public int reconnectionDelay = 1000;

    // [Tooltip("Maximum number of unread data messages in the queue. " +
    //          "New messages will be discarded.")]
    // public int maxUnreadMessages = 1;

    // Constants used to mark the start and end of a connection. There is no
    // way you can generate clashing messages from your serial device, as I
    // compare the references of these strings, no their contents. So if you
    // send these same strings from the serial device, upon reconstruction they
    // will have different reference ids.
    public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
    public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";

    // Internal reference to the Thread and the object that runs in it.
    protected Thread thread;
    protected SerialThreadLines serialThread;

    SerialPort data_stream = new SerialPort(portName, baudRate);
    public string receivedString;
    public GameObject testData;
    public Rigidbody rb;
    public float sensitivity = 0.01f;
    public string[] datas;
    // ------------------------------------------------------------------------
    // Invoked whenever the SerialController gameobject is activated.
    // It creates a new thread that tries to connect to the serial device
    // and start reading from it.
    // ------------------------------------------------------------------------
    void Start()
    {
        data_stream.Open();
    }

    // ------------------------------------------------------------------------
    // Invoked whenever the SerialController gameobject is deactivated.
    // It stops and destroys the thread that was reading from the serial device.
    // ------------------------------------------------------------------------
    --------------------------------------------------------------
    void Update()
    {
        receivedString = data_stream.ReadLine();
        datas = receivedString.Split(','); // [0] = joystick x [1] = joystick y [2] = button [3] = trigger
        // rb.AddForce();
        // rb.AddForce();
        // transform.Rotate(0,,); parameters for manipulating object
    }


}

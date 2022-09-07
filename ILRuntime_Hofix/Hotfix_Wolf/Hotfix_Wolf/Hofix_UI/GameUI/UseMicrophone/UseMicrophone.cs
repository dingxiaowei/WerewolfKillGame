using UnityEngine.UI;
using UnityEngine;

namespace Hotfix.UI
{
    [UI(ViewId.UseMicrophone, "UI/UseMicrophone/UseMicrophone")]
    public class UseMicrophone : UIBase
    {
        public UseMicrophone(string resourcePath) : base(resourcePath)
        {

        }

        [TransformPath("btn_Start")] private Button btn_Start;
        [TransformPath("btn_End")] private Button btn_End;
        [TransformPath("btn_Play")] private Button btn_Play;
        [TransformPath("txt_Tip")] private Text txt_Tip;

        private AudioSource aud;
        private bool isHaveMicrophone = false;
        private string[] devices;

        public override void Init()
        {
            base.Init();
            aud = transform.GetComponent<AudioSource>();

            btn_Start.onClick.AddListener(OnClickStartBtn);
            btn_End.onClick.AddListener(OnClickEndBtn);
            btn_Play.onClick.AddListener(OnClickPlayBtn);

            //获取麦克风设备，判断蛇摆是否有麦克风
            devices = Microphone.devices;
            if (devices.Length > 0)
            {
                isHaveMicrophone = true;
                txt_Tip.text = "设备有麦克风:" + devices[0];
            }
            else
            {
                isHaveMicrophone = false;
                txt_Tip.text = "设备没有麦克风";
            }
        }

        private void OnClickStartBtn()
        {
            if ((isHaveMicrophone == false) || (Microphone.IsRecording(devices[0])))
            {
                return;
            }
            //开始录音
            /*
             * public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency);
             * deviceName The name of the device.
             * loop Indicates whether the recording should continue recording if lengthSec is reached,
               and wrap around and record from the beginning of the AudioClip.
             * lengthSec Is the length of the AudioClip produced by the recording.
             * frequency The sample rate of the AudioClip produced by the recording.  
             */
            aud.clip = Microphone.Start(devices[0], true, 10, 44100);
        }

        private void OnClickEndBtn()
        {
            if ((isHaveMicrophone == false) || (!Microphone.IsRecording(devices[0])))
            {
                return;
            }
            //结束录音
            Microphone.End(devices[0]);
        }

        private void OnClickPlayBtn()
        {
            if ((isHaveMicrophone == false) || (Microphone.IsRecording(devices[0])
                || aud.clip == null))
            {
                return;
            }

            //播放录音
            aud.Play();
        }
    }
}

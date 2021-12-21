namespace HitopsCommon
{
    /// <summary>
    /// 2017.04.04 add by Ahn Jinsung
    /// System.Windows.Forms.Label 클래스를 쉽게 Replace로 치환하기 위환 Wrapper Class
    /// 이것을 치환한 이유는 GroupBox 를 DevExpress 의 GroupControl 로 치환시 안에 있는 Label AutoSize 속성이 True 이면
    /// 위치가 모두 깨지므로 기본적으로 AutoSize를 False 로하기 위함(부모 검사후 적용)
    /// </summary>
    public class Hitops_Label : System.Windows.Forms.Label
    {
        public Hitops_Label() : base()
        {
            if(this.Parent is DevExpress.XtraEditors.GroupControl && this.AutoSize ==true)
                this.AutoSize = false;
        }
    }
}

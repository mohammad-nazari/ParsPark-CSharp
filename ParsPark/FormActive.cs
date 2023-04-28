using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WebServiceLib;
using MetroFramework;
using MetroFramework.Forms;

namespace ParsPark
{
	public enum ActivationMethode { Online, Offline, Trial }
	public partial class FormActive : MetroForm
	{
		public ActivationClass ActivationObject { get; set; } = new ActivationClass();

		private string _errors;
		public bool IsActivated { get; set; }

		public FormActive()
		{
			InitializeComponent();
			IsActivated = false;
			DialogResult = DialogResult.No;
		}

		private void FormActive_Load(object sender, EventArgs e)
		{
			IsActivated = ActivationObject.IsSoftwareActivated();
			if (IsActivated == false)
			{
				ActivationObject.ReadInfoFromWindowsRegistry();
				if (ActivationObject.SerialNumber != null && ActivationObject.SerialNumber.Length == 20)
				{
					//this._activationObject.SerialNumber = this._activationObject.SerialNumber.ToUpper();
					mtxtSNo1.Text = ActivationObject.SerialNumber.Substring(0, 5);
					mtxtSNo2.Text = ActivationObject.SerialNumber.Substring(5, 5);
					mtxtSNo3.Text = ActivationObject.SerialNumber.Substring(10, 5);
					mtxtSNo4.Text = ActivationObject.SerialNumber.Substring(15, 5);
				}
				ActivationObject.GenerateGenerationCode();
				if (ActivationObject.GenerationCode != null && ActivationObject.GenerationCode.Length == 16)
				{
					//this._activationObject.GenerationCode = this._activationObject.GenerationCode.ToUpper();
					mtxtGen1.Text = ActivationObject.GenerationCode.Substring(0, 4);
					mtxtGen2.Text = ActivationObject.GenerationCode.Substring(4, 4);
					mtxtGen3.Text = ActivationObject.GenerationCode.Substring(8, 4);
					mtxtGen4.Text = ActivationObject.GenerationCode.Substring(12, 4);
				}
				txtCompany.Text = ActivationObject.ActivationData.Company;
				txtEmail.Text = ActivationObject.ActivationData.Email;
				txtName.Text = ActivationObject.ActivationData.User;
			}

			/////////////////
			/*this.mtxtSNo1.Text = "V6VLL";
			this.mtxtSNo2.Text = "OVU1S";
			this.mtxtSNo3.Text = "2XSXB";
			this.mtxtSNo4.Text = "T15ZG";

			this.txtName.Text = "moammad";
			this.txtCompany.Text = "anshan";
			this.txtEmail.Text = "mbn@gma.com";*/
			/////////////////
		}

		private void btnOnline_Click(object sender, EventArgs e)
		{

		}

		private void btnOffline_Click(object sender, EventArgs e)
		{

		}

		private bool FormValidation(ActivationMethode ActivationMetodeObject)
		{
			bool doAction = true;
			_errors = string.Empty;
			Regex regex = new Regex("^[a-zA-Z0-9]*$");
			ActivationObject.ActivationResult.ActivationInfo = new ActivationRequest();

			if (ActivationMetodeObject != ActivationMethode.Trial)
			{
				ActivationObject.SerialNumber = string.Empty;
				ActivationObject.SerialNumber += mtxtSNo1.Text.Trim();
				ActivationObject.SerialNumber += mtxtSNo2.Text.Trim();
				ActivationObject.SerialNumber += mtxtSNo3.Text.Trim();
				ActivationObject.SerialNumber += mtxtSNo4.Text.Trim();
				if (ActivationObject.SerialNumber.Length != 20)
				{
					_errors += Environment.NewLine + @"طول شماره سریال نامعتبر است.";
					doAction = false;
				}
				else
				{
					if (!regex.IsMatch(ActivationObject.SerialNumber))
					{
						_errors += Environment.NewLine + "شماره سریال نامعتبر است.";
						doAction = false;
					}
					else
					{
						ActivationObject.ActivationData.SerialNumber = ActivationObject.SerialNumber;
						ActivationObject.ActivationResult.ActivationInfo.SerialNumber = ActivationObject.SerialNumber;
					}
				}
			}
			ActivationObject.GenerationCode = string.Empty;
			ActivationObject.GenerationCode += mtxtGen1.Text.Trim();
			ActivationObject.GenerationCode += mtxtGen2.Text.Trim();
			ActivationObject.GenerationCode += mtxtGen3.Text.Trim();
			ActivationObject.GenerationCode += mtxtGen4.Text.Trim();
			if (ActivationObject.GenerationCode.Length != 16)
			{
				_errors += Environment.NewLine + "طول کد ایجاد شده نامعتبر است.";
				doAction = false;
			}
			else
			{
				if (!regex.IsMatch(ActivationObject.GenerationCode))
				{
					_errors += Environment.NewLine + "کد ایجاد شده نامعتبر است.";
					doAction = false;
				}
				else
				{
					ActivationObject.ActivationData.GenerationCode = ActivationObject.GenerationCode;
					ActivationObject.ActivationResult.ActivationInfo.GenerationCode = ActivationObject.GenerationCode;
				}
			}

			if (txtName.Text.Trim() == string.Empty)
			{
				_errors += Environment.NewLine + " لطفا نام خود را وارد کنید.";
				doAction = false;
			}
			else
			{
				ActivationObject.ActivationData.User = txtName.Text.Trim();
				ActivationObject.ActivationResult.ActivationInfo.User = txtName.Text.Trim();
			}

			if (txtCompany.Text.Trim() == string.Empty)
			{
				_errors += Environment.NewLine + " لطفا نام شرکت خود را وارد کنید.";
				doAction = false;
			}
			else
			{
				ActivationObject.ActivationData.Company = txtCompany.Text.Trim();
				ActivationObject.ActivationResult.ActivationInfo.Company = txtCompany.Text.Trim();
			}

			if (txtEmail.Text.Trim() == string.Empty)
			{
				_errors += Environment.NewLine + " لطفا ایمیل خود را وارد کنید.";
				doAction = false;
			}
			else if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
				@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase))
			{
				_errors += Environment.NewLine + " لطفا ایمیل معتبر وارد کنید.";
				doAction = false;
			}
			else
			{
				ActivationObject.ActivationData.Email = txtEmail.Text.Trim();
				ActivationObject.ActivationResult.ActivationInfo.Email = txtEmail.Text.Trim();
			}

			if (ActivationMetodeObject == ActivationMethode.Offline)
			{
				if (rtxtActivationCode.Text.Trim() == string.Empty)
				{
					_errors += Environment.NewLine + " لطفا کد فعالسازی را وارد کنید.";
					doAction = false;
				}
				else
				{
					ActivationObject.ActivationData.ActivationCode = rtxtActivationCode.Text.Trim();
					ActivationObject.ActivationResult.ActivationInfo.ActivationCode = rtxtActivationCode.Text.Trim();
				}
			}
			return doAction;
		}

		private void FillObjectData(ActivationMethode ActivationMethodeObject)
		{
			if (ActivationMethodeObject == ActivationMethode.Offline)
			{
				// Do activation operation
				// Send information to server
				ActivationObject.ActivationResult.ActivationInfo.SerialNumber = ActivationObject.SerialNumber;
				ActivationObject.ActivationResult.ActivationInfo.GenerationCode = ActivationObject.GenerationCode;
				ActivationObject.ActivationResult.ActivationInfo.ActivationCode = ActivationObject.ActivationCode;
				ActivationObject.ActivationResult.ActivationInfo.User = txtName.Text;
				ActivationObject.ActivationResult.ActivationInfo.Company = txtCompany.Text;
				ActivationObject.ActivationResult.ActivationInfo.Email = txtEmail.Text;
				ActivationObject.ActivationResult.ActivationInfo.Version = "1.0";
				ActivationObject.ActivationResult.ActivationInfo.Software = "ParsPark";
				ActivationObject.ActivationResult.ActivationInfo.Type = ActivationType.Full;
			}
			else
			{
				// Do activation operation
				// Send information to server
				ActivationObject.ActivationData.SerialNumber = ActivationObject.SerialNumber;
				ActivationObject.ActivationData.GenerationCode = ActivationObject.GenerationCode;
				ActivationObject.ActivationData.ActivationCode = ActivationObject.ActivationCode;
				ActivationObject.ActivationData.User = txtName.Text;
				ActivationObject.ActivationData.Company = txtCompany.Text;
				ActivationObject.ActivationData.Email = txtEmail.Text;
				ActivationObject.ActivationData.Version = "1.0";
				ActivationObject.ActivationData.Software = "ParsPark";
				ActivationObject.ActivationData.Type = ActivationType.Full;
			}
		}

		private void btnTrial_Click(object sender, EventArgs e)
		{

		}

		private void btnClose_Click(object sender, EventArgs e)
		{

		}
	}
}

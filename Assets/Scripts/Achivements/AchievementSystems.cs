using UnityEngine;
using System.Collections.Generic;

public sealed class AchievementSystems
{
		private static AchievementSystems instance = null;
		private static readonly object padlock = new object ();
		

		/// For achievement progress!
		private int _fapWaterMelonCount = 0;
		private int _fapAppleCount = 0;
		private int _fapOrangeCount = 0;
		private int _fapCaptureAssisinMonkeyCount = 0;
		private int _fapBombThrowCount = 0;
		private int _fapUseGodModeCount = 0;
		private int _fapPlayGameCount = 0;
		private int _fapSaveMeCount = 0;
		private int _fapUseDivineShieldCount = 0;
		AchievementSystems ()
		{

		}
		/// <summary>
		/// Gets A instance of Achievement System.
		/// </summary>
		/// <value>The instance.</value>
		public static AchievementSystems Instance {
				get {
						lock (padlock) {
								if (instance == null) {
										instance = new AchievementSystems ();
								}
								return instance;
						}
				}
		}

		/// <summary>
		/// Reads the data.
		/// </summary>
		public void ReadData ()
		{
				bool hasSavedData = false;
				if (PlayerPrefs.HasKey ("fapWaterMelonCount")) {
						_fapWaterMelonCount = PlayerPrefs.GetInt ("fapWaterMelonCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapAppleCount")) {
						_fapAppleCount = PlayerPrefs.GetInt ("fapAppleCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapOrangeCount")) {
						_fapOrangeCount = PlayerPrefs.GetInt ("fapOrangeCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapCaptureAssisinMonkeyCount")) {
						_fapCaptureAssisinMonkeyCount = PlayerPrefs.GetInt ("fapCaptureAssisinMonkeyCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapBombThrowCount")) {
						_fapBombThrowCount = PlayerPrefs.GetInt ("fapBombThrowCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapUseGodModeCount")) {
						_fapUseGodModeCount = PlayerPrefs.GetInt ("fapUseGodModeCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapPlayGameCount")) {
						_fapPlayGameCount = PlayerPrefs.GetInt ("fapPlayGameCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapSaveMeCount")) {
						_fapSaveMeCount = PlayerPrefs.GetInt ("fapSaveMeCount");
						hasSavedData = true;
				}
				if (PlayerPrefs.HasKey ("fapUseDivineShieldCount")) {
						_fapUseDivineShieldCount = PlayerPrefs.GetInt ("fapUseDivineShieldCount");
						hasSavedData = true;
				}
				if (!hasSavedData)
						SaveData ();
		}

		/// <summary>
		/// Save the Data
		/// </summary>
		public void SaveData ()
		{
				PlayerPrefs.SetInt ("fapWaterMelonCount", _fapWaterMelonCount);
				PlayerPrefs.SetInt ("fapAppleCount", _fapAppleCount);
				PlayerPrefs.SetInt ("fapOrangeCount", _fapOrangeCount);
				PlayerPrefs.SetInt ("fapCaptureAssisinMonkeyCount", _fapCaptureAssisinMonkeyCount);
				PlayerPrefs.SetInt ("fapBombThrowCount", _fapBombThrowCount);
				PlayerPrefs.SetInt ("fapUseGodModeCount", _fapUseGodModeCount);
				PlayerPrefs.SetInt ("fapPlayGameCount", _fapPlayGameCount);
				PlayerPrefs.SetInt ("fapSaveMeCount", _fapSaveMeCount);
				PlayerPrefs.SetInt ("fapUseDivineShieldCount", _fapUseDivineShieldCount);
				PlayerPrefs.Save ();
		}

		
		public int fapWaterMelonCount {
				get {
						return _fapWaterMelonCount;
				}
				set {
						_fapWaterMelonCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQEw", _fapWaterMelonCount / 1000, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapWaterMelonCount", _fapWaterMelonCount);
						PlayerPrefs.Save ();

				}
		}

		public int fapAppleCount {
				get {
						return _fapAppleCount;
				}
				set {
						_fapAppleCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQFQ", _fapAppleCount / 100, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapAppleCount", _fapAppleCount);
						PlayerPrefs.Save ();
				}
		}

		public int fapOrangeCount {
				get {
						return _fapOrangeCount;
				}
				set {
						_fapOrangeCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQFA", _fapOrangeCount / 1000, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapOrangeCount", _fapOrangeCount);
						PlayerPrefs.Save ();
				}
		}

		public int fapCaptureAssisinMonkeyCount {
				get {
						return _fapCaptureAssisinMonkeyCount;
				}
				set {
						_fapCaptureAssisinMonkeyCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQDQ", _fapCaptureAssisinMonkeyCount / 5, (bool pass) => {
								});
								Social.ReportProgress ("CgkIkJma0aEMEAIQFg", _fapCaptureAssisinMonkeyCount / 100, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapCaptureAssisinMonkeyCount", _fapCaptureAssisinMonkeyCount);
						PlayerPrefs.Save ();
				}
		}

		public int fapBombThrowCount {
				get {
						return _fapBombThrowCount;
				}
				set {
						_fapBombThrowCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQFw", _fapBombThrowCount / 2000, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapBombThrowCount", _fapBombThrowCount);
						PlayerPrefs.Save ();
				}
		}

		public int fapUseGodModeCount {
				get {
						return _fapUseGodModeCount;
				}
				set {
						_fapUseGodModeCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQEA", _fapUseGodModeCount / 10, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapUseGodModeCount", _fapUseGodModeCount);
						PlayerPrefs.Save ();
				}
		}

		public int fapPlayGameCount {
				get {
						return _fapPlayGameCount;
				}
				set {
						_fapPlayGameCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQEQ", _fapPlayGameCount / 100, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapPlayGameCount", _fapPlayGameCount);
						PlayerPrefs.Save ();
				}
		}

		public int fapSaveMeCount {
				get {
						return _fapSaveMeCount;
				}
				set {
						_fapSaveMeCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQEg", _fapSaveMeCount / 25, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapSaveMeCount", _fapSaveMeCount);
						PlayerPrefs.Save ();
				}
		}

		public int fapUseDivineShieldCount {
				get {
						return _fapUseDivineShieldCount;
				}
				set {
						_fapUseDivineShieldCount = value;
						#if UNITY_ANDROID
						if (Social.localUser.authenticated) {
								Social.ReportProgress ("CgkIkJma0aEMEAIQDw", _fapUseDivineShieldCount / 10, (bool pass) => {
								});
						}
						#endif
						PlayerPrefs.SetInt ("fapUseDivineShieldCount", _fapUseDivineShieldCount);
						PlayerPrefs.Save ();
				}
		}
}
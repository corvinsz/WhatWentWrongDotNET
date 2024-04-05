using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WhatWentWrongDotNET.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
	private static readonly PackIcon _closeIcon = new PackIcon() { Kind = PackIconKind.Close, Foreground = Brushes.Red };


	private readonly ISnackbarMessageQueue _snackbarMessageQueue;
	public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
	{
		this._snackbarMessageQueue = snackbarMessageQueue;
	}

	[ObservableProperty]
	private string _errorMessage;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(StartCommand))]
	private string _chosenFile;

	public bool CanStart => String.IsNullOrEmpty(ChosenFile) == false;

	[RelayCommand(CanExecute = nameof(CanStart))]
	private void Start()
	{
		ProcessStartInfo startInfo = new ProcessStartInfo
		{
			FileName = ChosenFile,
			RedirectStandardError = true
		};

		Process process = new Process()
		{
			StartInfo = startInfo,
			EnableRaisingEvents = true // Enable raising events for the process
		};

		try
		{
			process.Start();
			ErrorMessage = process.StandardError.ReadToEnd();
		}
		catch (Exception ex)
		{
			_snackbarMessageQueue.Enqueue(ex.Message, _closeIcon, null);
		}
	}

	[RelayCommand]
	private void ChooseFile()
	{
		ErrorMessage = string.Empty;
		OpenFileDialog ofd = new OpenFileDialog();
		if (ofd.ShowDialog() != true)
		{
			return;
		}

		ChosenFile = ofd.FileName;
	}
}

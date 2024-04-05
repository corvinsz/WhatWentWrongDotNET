using MaterialDesignThemes.Wpf;
using System.Runtime.CompilerServices;
using WhatWentWrongDotNET.ViewModels;

namespace WhatWentWrongDotNET.Tests;

public class MainWindowViewModelTests
{
	[Theory]
	[InlineData(null, false)]
	[InlineData("", false)]
	[InlineData("someFilePath.txt", true)]
	public void StartCommand_CanBeExecuted_WhenFileIsSelected(string file, bool expected)
	{
		//Arrange
		MainWindowViewModel viewModel = new MainWindowViewModel(null);

		//Act
		viewModel.ChosenFile = file;

		//Assert
		Assert.Equal(expected, viewModel.StartCommand.CanExecute(null));
	}
}
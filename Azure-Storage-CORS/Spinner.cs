﻿using System;
using System.Text;

namespace AzureStorageCORS
{
	public class Spinner
	{
		private int _currentAnimationFrame;

		public Spinner()
		{
			SpinnerAnimationFrames = Console.OutputEncoding is UTF8Encoding ?
				new[]
				{
					'◐',
					'◓',
					'◑',
					'◒'
				}
				:
				new[]
				{
					'|',
					'/',
					'-',
					'\\'
				};
		}

		public char[] SpinnerAnimationFrames { get; set; }

		public void UpdateProgress()
		{
			// Hide cursor to improve the visuals (note: we should re-enable at some point)
			Console.CursorVisible = false;

			// Store the current position of the cursor
			var originalX = Console.CursorLeft;
			var originalY = Console.CursorTop;

			// Write the next frame (character) in the spinner animation
			Console.Write(SpinnerAnimationFrames[_currentAnimationFrame]);

			// Keep looping around all the animation frames
			_currentAnimationFrame++;
			if (_currentAnimationFrame == SpinnerAnimationFrames.Length)
			{
				_currentAnimationFrame = 0;
			}

			// Restore cursor to original position
			Console.SetCursorPosition(originalX, originalY);
		}

		public void Reset()
		{
			Console.CursorVisible = true;
		}

	}
}


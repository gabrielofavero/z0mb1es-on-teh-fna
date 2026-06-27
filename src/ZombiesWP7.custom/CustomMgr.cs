using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ZombiesWP7.custom;

public class CustomMgr
{
	private ICollection<MediaSource> mediaSources;

	private MediaLibrary mediaLib;

	private PictureCollection picCollection;

	private List<CustomSrc> srcImages;

	public CustomImg[] customImg;

	private GraphicsDevice gd;

	private int[] gameCustomMap;

	private int totalCustoms;

	private void SetGameCustomMap()
	{
		totalCustoms = 0;
		for (int i = 0; i < customImg.Length; i++)
		{
			if (customImg[i].enabled)
			{
				gameCustomMap[totalCustoms] = i;
				totalCustoms++;
			}
		}
	}

	public int GetGameCustomIdx(int i)
	{
		return gameCustomMap[i % totalCustoms];
	}

	public bool GetAnyCustoms()
	{
		return totalCustoms > 0;
	}

	public Texture2D GetSrcTexture(int idx)
	{
		if (idx < srcImages.Count)
		{
			return srcImages[idx].GetTexture();
		}
		return null;
	}

	public CustomImg GetCustomImageFromSrcIdx(int idx)
	{
		int imgIdx = srcImages[idx].imgIdx;
		if (imgIdx > -1)
		{
			return customImg[imgIdx];
		}
		return null;
	}

	public int GetSrcImageCount()
	{
		if (srcImages == null)
		{
			return 0;
		}
		return srcImages.Count;
	}

	public bool GetSrcEnabled(int idx)
	{
		if (srcImages[idx].imgIdx > -1)
		{
			return customImg[srcImages[idx].imgIdx].enabled;
		}
		return false;
	}

	public CustomMgr()
	{
		customImg = new CustomImg[5];
		gameCustomMap = new int[customImg.Length];
		for (int i = 0; i < customImg.Length; i++)
		{
			customImg[i] = new CustomImg();
		}
	}

	public void FreeCustom(int idx)
	{
		srcImages[idx].imgIdx = -1;
		for (int i = 0; i < customImg.Length; i++)
		{
			if (customImg[i].srcIdx == idx)
			{
				customImg[i].srcIdx = -1;
				customImg[i].enabled = false;
			}
		}
		SetGameCustomMap();
	}

	public bool ProcessCustom(int idx)
	{
		int num = -1;
		for (int i = 0; i < customImg.Length; i++)
		{
			if (customImg[i].srcIdx == idx && customImg[i].enabled)
			{
				num = i;
			}
		}
		if (num == -1)
		{
			for (int j = 0; j < customImg.Length; j++)
			{
				if (!customImg[j].enabled)
				{
					num = j;
					customImg[j].enabled = true;
					break;
				}
			}
		}
		if (num > -1)
		{
			srcImages[idx].imgIdx = num;
			customImg[num].srcIdx = idx;
			customImg[num].Process(srcImages[idx], gd);
			SetGameCustomMap();
			return true;
		}
		SetGameCustomMap();
		return false;
	}

	public void Init(GraphicsDevice GraphicsDevice, ContentManager Content)
	{
		bool flag = false;
		gd = GraphicsDevice;
		srcImages = new List<CustomSrc>();
		try
		{
			mediaSources = MediaSource.GetAvailableMediaSources();
		}
		catch (NotImplementedException)
		{
			flag = true;
			mediaSources = new List<MediaSource>();
		}
		bool flag2 = false;
		if (mediaSources.Count > 0)
		{
			foreach (MediaSource mediaSource in mediaSources)
			{
				mediaLib = new MediaLibrary(mediaSource);
				picCollection = mediaLib.Pictures;
				if (picCollection.Count <= 0)
				{
					continue;
				}
				foreach (Picture item in picCollection)
				{
					try
					{
						if (!flag2)
						{
							item.GetThumbnail();
							flag2 = true;
						}
						srcImages.Add(new CustomSrc(item.Name + item.Date, gd, item));
					}
					catch
					{
						flag = true;
					}
				}
			}
		}
		if (flag)
		{
			srcImages = new List<CustomSrc>();
			srcImages.Add(new CustomSrc("test1", Content.Load<Texture2D>("gfx/custom/test1")));
			srcImages.Add(new CustomSrc("test2", Content.Load<Texture2D>("gfx/custom/test2")));
			srcImages.Add(new CustomSrc("test3", Content.Load<Texture2D>("gfx/custom/test3")));
			srcImages.Add(new CustomSrc("test4", Content.Load<Texture2D>("gfx/custom/test4")));
			srcImages.Add(new CustomSrc("test5", Content.Load<Texture2D>("gfx/custom/test5")));
			srcImages.Add(new CustomSrc("test6", Content.Load<Texture2D>("gfx/custom/test6")));
			srcImages.Add(new CustomSrc("test7", Content.Load<Texture2D>("gfx/custom/test7")));
			srcImages.Add(new CustomSrc("test8", Content.Load<Texture2D>("gfx/custom/test8")));
		}
	}

	internal void Write(BinaryWriter w)
	{
		for (int i = 0; i < customImg.Length; i++)
		{
			customImg[i].Write(w);
		}
	}

	internal void Read(BinaryReader r)
	{
		for (int i = 0; i < customImg.Length; i++)
		{
			customImg[i].Read(r);
		}
		foreach (CustomSrc srcImage in srcImages)
		{
			srcImage.imgIdx = -1;
		}
		for (int j = 0; j < customImg.Length; j++)
		{
			if (!customImg[j].enabled)
			{
				continue;
			}
			bool flag = false;
			for (int k = 0; k < srcImages.Count; k++)
			{
				if (srcImages[k].ID == customImg[j].ID)
				{
					srcImages[k].imgIdx = j;
					customImg[j].srcIdx = k;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				customImg[j].enabled = false;
			}
			else
			{
				ProcessCustom(customImg[j].srcIdx);
			}
		}
	}
}

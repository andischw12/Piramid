Thank you for buying Deckard ChromaKeyer!
Deckard UniKeyer is a production quality system for doing quality Keying inside Unity. It is made to make easy otherwise difficult task of doing perfect chromakeys and is mostly dedicated to Virtual Production workflows.

Gereral things to know before doing some good chromakeying
Good chromakey requres good source footage. There are few guidelines hoiw to obtain good source images out of your camera. 
-Choosing a right camera
Greenscreen requires a good camera and you will never get perfect results with a simple webcam. Webcams have a lot of noise, low dynamic range, do some image pre-processing and auto adjust their features and this can and will affect overall quality of your keying. Best way to obtain good input video is to use DSLR or dedicated Video Camera for this task.  There are some pretty affordable solutions available, and even a cheap second hand DSLR will do a great job. I'm personally using a Blackmagic Pocket Cinema 4k camera. It is incredibly versatile budget camera that has some important features required for good keying: Manual control, high dynamic range and low latency. Unfortunately, even if it captures at 4k, it doesn't output 4k HDMI signal (it can output fullHD resolution - that should be enough in most cases). Tip: when Using BMPCC4K set its profile to Extended Video - this will give you an optimal dynamic range for best keying. 

-Capturing with video capture card
Your camera or DSLR probably doesn't have a webcamera interface, so you will also need a video capture device that has HDMI (or SDI) input and can act as a webcam device. I'm using mostly Blackmagic Desing Intensity Pro 4k card (or Decklink) for this task, but there are many other available solutions on a market. 

-Well lit greenscreen
Greenscreen should always be lit evenly as possible. Avoid having any wrinkles on your greenscren material. UniKeyer works pretty well even with unevenly lit screens, but this works mostly for cases when you are using a static camera. When you are using moving camera, well lit screen can be really important. Deckard UniKeyer is made to deal pretty well with all extreme cases and features many tools to do some good keying even with a problematic background. 



Getting Started with UniKeyer
Once imported into your project you can add it to your scene by Right Clicking on hierarchy and going to Deckard UniKeyer menu and selecting Add ChromaKey Rig. This will add a special chromakey rig that contains all neccessary scripts and objects. 
Next steps will be to configure your chromakeyer. 

-Keying Profile
Here you can create or assign existing Chroma Keying profile. Setting a profile is somewhat similar to setting post processing profiles in Unity. But in this case, a "profile" is in a form of a material with it's shader. Once you create or assign an existing profile, you can then edit material properties - keying properties. But before you start doing so, you will want to set up a source of your video stream. 
-Source Type
	-Webcam: it will use webcam device or capture card
	-Render Texture: it will use a render texture - this is usefull when you are using third party video capture plugins like AV Pro Live,AV Pro Decklink or when you are using Video player with render texture as a target.
	-Video: you should use this if you are using pre-recorded video file. 

-Key Type:
We have two key types, one is a standard keying type where you input a color for keying out background(for moving camera), and another is a keying with a Clean Mate Source (when your camera doesn't move). 

Capture Clean mate
What is Clean mate source? It is a simple image of a background without any objects, for example: just a unevenly lit green screen. This type of keying can produce best results in a non perfecly lit environment. You can sample this image and save it as a texture in Unity by clicking on Capture Clean Mate. 
Note: it will always save image with a same name: ultimatte.png If you are using more than one profile in your unity project, be sure to rename it after capture. This behaviour is here so that you can update clean mate image across many Unity scenes in one single run. But there might be some other cases when you are using multiple cameras, where you probably want to rename those sample mate images. 

Custom Mask
 Sometimes you probably need to mask some zones of your screen so here you can add a texture with masking zones. Black color is a masked color. 

Setting a Key Profile (keying): 
When using ost important values are Key Colors (there are two of them for better flexibility of keying if a), Threshold and ThresholdLow. This is where you are setting a range of color similarity to key out. Once you have those values perfectly set, you can tune other parameters. When keying a full figure, you want to have a full control on how you want to treat your keying. UniKeyer is capable of doing soft keys (where you want to preserve shadows and preserve real shadows or semi transparent objects. But sometimes you actually want to preserve smooth transition behaviour on upper part of a body (where your actor may use glasses, hold bottles or transparent products) but you want to eliminate any shadows that are casted on the ground. So here comes a need for separate Threshold parameters, one that works for the upper part of body, and one for lower. In this way you can tune differenlty both parts of an image.

Despill Chroma and Despill luma defines how the borders will behave and how a despilling (removage of green cast from a screen behaves). Despil Chroma should mostly be set to 1 but a despil Luma should be set on a base of your virtual background. White and bright backgrounds require positive values, dark baskgrounds darker values. 
Border: defines how mush smoothing has to be applied to borders of a mask. This can help to have a natural defocused borders, and it can clean up borders. 

 
 Additional passes:
This is one of the interesting features of UniKeyer. Additional passes give you an ability to add multiple layers of chromaKeying (for other colors) or to add some special filters ie. It can be used for generating some effects like holographic effects, edge refinement shaders, beauty filters and so on. NOTE: additional apsses can have impact on performance as any image effects - they should be used with care and mostly for high quality applications. 


Image Tracking: 
One of the features of Deckard UniKeyer is it's ability to track figures. It uses a simple enough tracking to be able to run in realtime without big performance overhead, while giving an incredibly stable figure tracking. It can track extents of a figure, head Top and shoulders position. This tracking data can then be easily used with Cinemachine for virtual camera guidance, with robotic systems for automated camera movement, for alerting system when a figure is aproaching borders of a camera capture area, for activating behaviours (a virtual touch screen), or for example, positioning feets on a floor level and generating correct contact feet points for placing shadows. This system is in developement, and there may be many future uses for it. 


Full manual and tutorials are available on: 
https://olivr.info/deckard-chromakey-manual/

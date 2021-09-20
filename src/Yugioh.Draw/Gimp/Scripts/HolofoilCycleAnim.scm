; ********************************************************************
; 
; This program is free software; you can redistribute it and/or modify
; it under the terms of the GNU General Public License as published by
; the Free Software Foundation; either version 2 of the License, or
; (at your option) any later version.
; 
; This program is distributed in the hope that it will be useful, but
; WITHOUT ANY WARRANTY; without even the implied warranty of
; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
; 
; See the GNU General Public License for more details.
; 
; ********************************************************************

(define (script-fu-holofoil-cycle old-image old-drawable frames)

  ; only execute the script if an even number of frames was chosen
  (if (= (remainder frames 2) 0)

    (let* (	
	    (height (car (gimp-image-height old-image)))
		(width (car (gimp-image-width old-image)))
		; Create the new image using width and height of old
		(new-image (car (gimp-image-new width height RGB)))
		; Create a new layer for the new image
		(first-holofoilframe (car (gimp-layer-new new-image
							width height
							RGBA-IMAGE
							"holofoil-frame"
							100
							NORMAL-MODE)))
		(first-artworkframe (car (gimp-layer-new new-image
							width height
							RGBA-IMAGE
							"artwork-frame"
							100
							NORMAL-MODE)))
		(new-selection (car (gimp-channel-new new-image
							width height
							"selection"
							100
							'(0 0 0))))
		(old-selection)
		(float)
		(hue-step (/ 360 frames))
		(half (/ frames 2)))

	(gimp-image-undo-freeze old-image)
	(gimp-image-undo-disable new-image)

	; save the old image's selection to a channel
	; and copy it to a new channel in the new image
	(set! old-selection (car (gimp-selection-save old-image)))
	(gimp-selection-none old-image)
	(gimp-image-add-channel new-image new-selection 0)
	(gimp-edit-copy old-selection)
	(set! float (car (gimp-edit-paste new-selection 1)))
	(gimp-floating-sel-anchor float)

	; the first frame contains the visible contents of the old image
	(gimp-image-add-layer new-image first-artworkframe 0)
	(gimp-image-add-layer new-image first-holofoilframe 0)

	(gimp-edit-copy (car (gimp-image-get-layer-by-name old-image "artwork")))
	(gimp-floating-sel-anchor (car (gimp-edit-paste first-artworkframe TRUE)))
	
	(gimp-edit-copy (car (gimp-image-get-layer-by-name old-image "holofoil")))
	(gimp-floating-sel-anchor (car (gimp-edit-paste first-holofoilframe TRUE)))
	
	;(gimp-layer-set-mode first-holofoilframe LAYER-MODE-MULTIPLY)
	(gimp-layer-set-mode first-holofoilframe LAYER-MODE-GRAIN-EXTRACT)

	; cycle the hue using the specified number of layers
	(let loop ((i 1))
	  (if (< i frames)
	    (let* (
				(hue-offset (round (* (- (remainder (+ i half) frames) half) hue-step)))
				(new-artworkframe (car (gimp-layer-copy first-artworkframe 0)))
				(new-holofoilframe (car (gimp-layer-copy first-holofoilframe 0)))
			  )
			(gimp-image-add-layer new-image new-artworkframe 0)
			(gimp-image-add-layer new-image new-holofoilframe 0)

			(gimp-hue-saturation new-holofoilframe 0 hue-offset 0 0)
			(gimp-image-merge-down new-image new-holofoilframe EXPAND-AS-NECESSARY)
			(loop (+ i 1))
		)
	  )
	)
	
	;(gimp-hue-saturation first-holofoilframe 0 0 80 0)
	(gimp-image-merge-down new-image first-holofoilframe EXPAND-AS-NECESSARY)


	; restore the old image's selection and cleanup
	(gimp-selection-load old-selection)
	(gimp-selection-none new-image)
	(gimp-image-remove-channel old-image old-selection)
	(gimp-image-remove-channel new-image new-selection)
	(gimp-image-undo-thaw old-image)
	(gimp-image-undo-enable new-image)
	(gimp-display-new new-image)
	(gimp-displays-flush))))


(script-fu-register
  "script-fu-holofoil-cycle"
  "<Image>/Filters/Animation/Holofoil Cycle"
  "Create an animation that shifts the selected portion of the image's hue in each frame."
  "abstractx1"
  "abstractx1"
  "September 2021"
  "RGB*"
  SF-IMAGE "Image" 0
  SF-DRAWABLE "Drawable" 0 ;not used
  SF-ADJUSTMENT "Frames (must be even)" '(60 2 120 2 2 0 1))
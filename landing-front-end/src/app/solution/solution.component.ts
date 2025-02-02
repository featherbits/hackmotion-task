import { Component, signal, WritableSignal } from '@angular/core';

interface ITimestampedContent {
  startTime: number
  title: string
  content: string
  open: WritableSignal<boolean>
}

@Component({
  selector: 'app-solution',
  imports: [],
  templateUrl: './solution.component.html',
  styleUrl: './solution.component.scss'
})
export class SolutionComponent {

  public readonly duration = signal(0)
  public readonly timestampedContent: ITimestampedContent[] = [
    {
      startTime: 0,
      title: 'Static top drill',
      content: 'Get a feel for the optimal wrist position at Top of your swing',
      open: signal(true)
    },
    {
      startTime: 11,
      title: 'Dynamic top drill',
      content: 'Dynamically train your wrist position at Top',
      open: signal(false)
    },
    {
      startTime: 22,
      title: 'Top full swing challenge',
      content: 'Train your maximum power swing',
      open: signal(false)
    }
  ]

  private activeItem: ITimestampedContent = this.timestampedContent[0]

  updateVideoProgress(video: HTMLVideoElement): void {
    window.requestAnimationFrame(() => {
      const currentTime = video.currentTime
      this.duration.set(currentTime / video.duration * 100)

      let candidate: ITimestampedContent | null = null

      for (let item of this.timestampedContent) {
        if (item.startTime <= currentTime) {
          candidate = item
        } else break
      }

      if (candidate && candidate !== this.activeItem) {
        this.activeItem.open.set(false)
        candidate.open.set(true)
        this.activeItem = candidate
      }

      if (!video.paused) this.updateVideoProgress(video)
    })
  }
}

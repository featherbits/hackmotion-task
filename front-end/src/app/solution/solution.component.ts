import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

interface ITimestampedContent {
  startTime: number
  title: string
  content: string
  open: WritableSignal<boolean>
}

const parOptionMap:{[index:string]:string} = {
  'break-par': 'Break Par',
  'break-80': 'Break 80',
  'break-90': 'Break 90',
  'break-100': 'Break 100',

}

@Component({
  selector: 'app-solution',
  imports: [],
  templateUrl: './solution.component.html',
  styleUrl: './solution.component.scss'
})
export class SolutionComponent implements OnInit {

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
  public readonly parOption = signal('')

  private activeItem: ITimestampedContent = this.timestampedContent[0]

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const par:string = this.route.snapshot.params['par']
    this.parOption.set(parOptionMap[par])
  }

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

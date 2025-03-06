import type { TaskItem } from '../types/types'

const lotColorMap = new Map<string, string>()
const pastelColors = [
  '#7ACDA0', // 민트
  '#F2938C', // 코랄
  '#95A8D8', // 라벤더
  '#F0B77A', // 피치
  '#B0CC8E', // 라임
  '#E88B8B', // 살몬
  '#B991C8', // 보라
  '#7EB99F', // 녹색
  '#E8C956', // 머스타드
  '#6FA7C1', // 하늘색
  '#E5A886', // 살구색
  '#B39EB5', // 자주
  '#8CB07C', // 올리브
  '#EB9486', // 테라코타
  '#9FB0BF', // 슬레이트
  '#D6927C', // 황토색
  '#82B0D9', // 청색
  '#D6CE7B', // 황색
  '#C48F9C', // 로즈
  '#B2AA8E', // 베이지
  '#7EC4C1', // 청록
  '#D1A0A0', // 장미색
  '#8AAD71', // 녹색
  '#BE99DB', // 라일락
  '#D3AD99', // 카라멜
  '#7BA3C6', // 프러시안블루
  '#A0C28C', // 옥색
  '#EB9F9F', // 샐몬핑크
  '#92A9C8', // 스틸블루
  '#9FC088', // 아스파라거스
]

const generatePastelColor = (seed: string): string => {
  // 문자열에서 해시값 생성
  let hash = 0
  for (let i = 0; i < seed.length; i++) {
    hash = seed.charCodeAt(i) + ((hash << 5) - hash)
  }

  // 색조(Hue): 0-360 범위의 값
  const hue = Math.abs(hash % 360)

  // 진한 파스텔톤을 위한 채도와 명도 설정
  // 채도(Saturation): 45-65% - 더 선명하게
  // 명도(Lightness): 60-75% - 약간 더 어둡게
  const saturation = 45 + Math.abs((hash >> 8) % 20)
  const lightness = 60 + Math.abs((hash >> 16) % 15)

  return `hsl(${hue}, ${saturation}%, ${lightness}%)`
}

export const getLotColor = (task: TaskItem): string => {
  if (lotColorMap.has(task.LOT_ID)) {
    return lotColorMap.get(task.LOT_ID)
  }
  // 방법 1: 미리 정의된 파스텔 색상 팔레트에서 색상 할당
  // lotColorMap.size를 이용하여 순차적으로 색상 할당, 팔레트를 넘어가면 다시 처음부터
  const colorFromPalette = pastelColors[lotColorMap.size % pastelColors.length]

  // 방법 2: 동적으로 파스텔 색상 생성
  // 충분한 색상 다양성을 위해 LOT_ID를 기반으로 색상 생성
  const dynamicColor = generatePastelColor(task.LOT_ID)

  // 두 방법 중 선택 (여기서는 미리 정의된 팔레트와 동적 생성을 혼합하여 사용)
  // 30개 이내는 팔레트 사용, 그 이상은 동적 생성
  const color = lotColorMap.size < pastelColors.length ? colorFromPalette : dynamicColor

  lotColorMap.set(task.LOT_ID, color)

  return color
}

<template>
  <q-layout view="hHh LpR fFf">
    <q-header elevated class="sk-header">
      <q-toolbar>
        <q-btn dense flat round icon="menu" @click="toggleLeftDrawer" />
        <q-toolbar-title>
          <q-avatar>
            <img src="https://cdn.quasar.dev/logo-v2/svg/logo-mono-white.svg" />
          </q-avatar>
          FAB PHOTO Schduler
        </q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-drawer show-if-above v-model="leftDrawerOpen" side="left" behavior="desktop" elevated>
      <q-list>
        <!-- 1. 기준정보 -->
        <q-expansion-item
          expand-separator
          icon="table_chart"
          label="기준 정보"
          @show="onShow('masterData')"
          @hide="onHide('masterData')"
          :header-style="parentMenuBackgroundColor.get('masterData')"
        >
          <q-item clickable v-ripple to="/master/bom" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="table_chart" />
            </q-item-section>
            <q-item-section>BOM 정보</q-item-section>
          </q-item>

          <q-item clickable v-ripple to="/master/equipment" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="map" />
            </q-item-section>
            <q-item-section>설비 정보</q-item-section>
          </q-item>

          <q-item clickable v-ripple to="/master/lot" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="widgets" />
            </q-item-section>
            <q-item-section>Lot 정보</q-item-section>
          </q-item>

          <q-item clickable v-ripple to="/master/info" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="info" />
            </q-item-section>
            <q-item-section>기타 정보</q-item-section>
          </q-item>
        </q-expansion-item>
        <q-expansion-item
          expand-separator
          icon="event"
          label="생산 계획"
          @show="onShow('productionPlan')"
          @hide="onHide('productionPlan')"
          :header-style="parentMenuBackgroundColor.get('productionPlan')"
        >
          <q-item clickable v-ripple to="/planning/engine-execute" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="play_arrow" />
            </q-item-section>
            <q-item-section>생산 계획 엔진 실행</q-item-section>
          </q-item>
          <q-item clickable v-ripple to="/schedule/eqp" class="q-pl-lg">
            <q-item-section avatar>
              <!-- timeline memory -->
              <q-icon name="build" />
            </q-item-section>
            <q-item-section>설비별 생산 계획</q-item-section>
          </q-item>
        </q-expansion-item>

        <!-- 3. 계획 분석 (하위 메뉴 포함) -->
        <q-expansion-item
          expand-separator
          icon="bar_chart"
          label="계획 분석"
          @show="onShow('planAnalysis')"
          @hide="onHide('planAnalysis')"
          :header-style="parentMenuBackgroundColor.get('planAnalysis')"
        >
          <q-item clickable v-ripple to="/analysis/gantt" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="timeline" />
            </q-item-section>
            <q-item-section>간트차트</q-item-section>
          </q-item>
          <q-item clickable v-ripple to="/analysis/gantt2" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="timeline" />
            </q-item-section>
            <q-item-section>간트차트2</q-item-section>
          </q-item>
          <q-item clickable v-ripple to="/analysis/eqp-schedule" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="compare" />
            </q-item-section>
            <q-item-section>계획 결과 비교</q-item-section>
          </q-item>
          <q-item clickable v-ripple to="/analysis/dispatch-log" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="list" />
            </q-item-section>
            <q-item-section>Dispatch Log</q-item-section>
          </q-item>
          <q-item clickable v-ripple to="/analysis/other-logs" class="q-pl-lg">
            <q-item-section avatar>
              <q-icon name="description" />
            </q-item-section>
            <q-item-section>기타 Log</q-item-section>
          </q-item>
        </q-expansion-item>
      </q-list>
    </q-drawer>
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const leftDrawerOpen = ref(false)
function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value
}

const parentMenuBackgroundColor = ref(new Map<string, any>())
const onShow = (menu: string) => {
  parentMenuBackgroundColor.value.set(menu, { backgroundColor: 'rgba(0, 128, 255, 0.2)' })
}
const onHide = (menu: string) => {
  parentMenuBackgroundColor.value.delete(menu)
}
</script>
<style scoped>
.sk-header {
  background: linear-gradient(90deg, #ea002c 0%, #ff6a00 100%); /* SK하이닉스 로고 그라디언트 */
  color: white;
}
</style>

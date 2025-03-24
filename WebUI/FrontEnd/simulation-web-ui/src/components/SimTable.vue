<template>
  <div class="sim-table-container q-mt-md q-px-md">
    <q-table
      class="sim-table"
      :rows="data"
      :columns="columns"
      :loading="loading"
      :row-key="rowKey"
      selection="single"
      bordered
      flat
      dense
      v-model:selected="selectedTableRows"
      @row-click="onTableRowClick"
      sticky-header
      virtual-scroll
      :virtual-scroll-slice-size="visibleRows"
      :pagination="pagination"
      :style="tableStyle"
      :table-row-class-fn="tableRowClassFn"
    >
      <template v-slot:loading>
        <q-inner-loading showing color="primary" />
      </template>
      <template v-slot:no-data>
        <div class="full-width row flex-center q-pa-md text-grey-8">데이터가 없습니다</div>
      </template>

      <!-- 필요한 경우 사용자 정의 슬롯 추가 -->
      <slot></slot>
    </q-table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import type { QTableColumn } from 'quasar'
import type { PropType } from 'vue'

// Props 정의
const props = defineProps({
  data: {
    type: Array,
    required: true,
  },
  columns: {
    type: Array as () => QTableColumn[],
    required: true,
  },
  loading: {
    type: Boolean,
    default: false,
  },
  visibleRows: {
    type: Number,
    default: 6,
  },
  tableHeight: {
    type: Number,
    default: 350,
  },
  offsetHeight: {
    type: Number,
    default: 200,
  },
  useFixedHeight: {
    type: Boolean,
    default: false,
  },
  fixedHeight: {
    type: Number,
    default: 350,
  },
  rowKey: {
    type: String,
  },
  tableRowClassFn: {
    type: Function as PropType<(row: any) => string>,
    default: () => '',
  },
})

// 이벤트 정의
defineEmits(['update:selected', 'row-click'])

// 페이지네이션 설정
const pagination = ref({
  rowsPerPage: 0, // 0으로 설정하면 페이지네이션 비활성화
  sortBy: '',
  descending: false,
})

const selectedTableRows = ref([])

const tableStyle = computed(() => {
  if (props.useFixedHeight) {
    return {
      height: `${props.fixedHeight}px`,
      maxHeight: `${props.fixedHeight}px`,
    }
  } else {
    const height = `calc(100vh - ${props.offsetHeight}px)`
    return {
      height: height,
      maxHeight: height,
    }
  }
})

const onTableRowClick = (evt, row) => {
  selectedTableRows.value = [row]
}

onMounted(() => {})
</script>

<style scoped>
.sim-table-container {
  width: 100%;
}

.sim-table {
  overflow-y: auto;
}

/* 테이블 헤더 고정 CSS */
:deep(.q-table) thead {
  position: sticky;
  top: 0;
  z-index: 1;
}

:deep(.q-table) thead tr th {
  position: sticky;
  top: 0;
  z-index: 1;
  background-color: #f0f0f0;
}

/* 테이블 헤더 스타일 */
:deep(.q-table thead tr) {
  background-color: #f0f0f0;
}

/* 고정 헤더에 그림자 효과 추가 */
:deep(.q-table--sticky-header thead tr:last-child th) {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

/* 선택된 행 스타일 */
:deep(.q-table tbody tr.selected) {
  background-color: rgba(25, 118, 210, 0.12);
}

/* 체크박스 열 숨기기 */
:deep(.q-table--col-auto-width) {
  display: none;
}

:deep(.q-checkbox) {
  display: none;
}

/* 열 사이에 경계선 추가 */
:deep(.q-table th),
:deep(.q-table td) {
  border-right: 1px solid #e0e0e0;
}

/* 마지막 열의 오른쪽 경계선은 제거 */
:deep(.q-table th:last-child),
:deep(.q-table td:last-child) {
  border-right: none;
}
:deep(.q-table tr:last-child td) {
  border-bottom-width: 1px;
}

/* 테두리 스타일을 일관되게 유지 */
:deep(.q-table) {
  border-collapse: collapse;
}
</style>

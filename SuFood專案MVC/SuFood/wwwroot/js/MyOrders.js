var app = new Vue({
    el:".MyOrders",
    data:
    {
        title: "我的訂單",
        od: [], //orderdetail  你要抓的model值丟這裡
        toast: "", //新增成功用
        ordersId: undefined,  //要新的Id就新增在這邊
        CreateOrEditOrDelete: "",
        keyword:"",
        popupShowing: {
            showPopup:false
        },
        toastHintStyle: {
            fadeInUp:false
        },
        //新增評論用
        createCommentList: {
            reviewId: 0,
            ratingStar: 0,
            comment: "",
            ordersId: 0
        },
        modalContentStyle: {
            w200: true,
            w800: false
        },
        modalContainerStyle: {
            showModal: false,
        },
        isActive: false,
    },
    request:  //你要抓的model值放這邊
    {
        ordersId:0,
    },
    methods:
    {
        toastHint() {
            this.toastHintStyle.fadeInUp = true;
            setTimeout(() => {
                this.toastHintStyle.fadeInUp = false
            },2000)
        },
        createComment() {
            this.CreateOrEditOrDelete = 'Create';
            this.popupShowing.showPopup = true;
        },
        CloseComment() {
            /*console.log('123')*/
            this.popupShowing.showPopup = false;
        },
        closepopupShowHint() {
            this.popupShowing.showPopup = false;
            this.toastHint();
        },
        openModal() {
            this.modalContainerStyle.showModal = true;
        },
        closeModal() {
            this.modalContainerStyle.showModal = false;
        },
        //取得後台資料路由丟這邊
        GetDetail() {
            axios.get("").then(response => {
                this.od = response.data
            })
        },
        CreateComment(createCommentList) {
            let _this = this;
            var request = null;
            request = {
                "reviewId": _this.createCommentList.reviewId,
                "ratingStar": _this.createCommentList.ratingStar,
                "comment": _this.createCommentList.comment,
                "ordersId": _this.createCommentList.ordersId
            }
            if (this.createCommentList.ordersId == 0 || this.createCommentList.ratingStar > 6 || this.createCommentList.ratingStar < 0) {
                alert('客戶ID為必填欄位,星數不得大於5或是小於0')
            }
            else {
                axios.post('MyOrders/CreateComment', request
                ).then(response => {
                    _this.toast = response.data
                    /*console.log('123')*/
                    this.closepopupShowHint()
                    _this.GetComments()
                })
            }
        },
    },
    //篩選
    computed: {
        filter() {
            arr = this.od.filter(o => {
                return o.od.indexOf(this.keyword) != -1
            })
        }
    },
    //抓資料
    mounted() {
        this.GetDetail();
    }
})
    var app = new Vue({
    el:".MyOrders",
    data:
    {
        recyleSubscribeOrders:[],
        recomment: [],
        ordersId:0,
        title: "我的訂單",
        od: {
            comments: [],
        }, //orderdetail  你要抓的model值丟這裡
        toast: "", //新增成功用 塞字串
        CreateOrEditOrDelete: "",
        keyword: "",
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
            ordersId:0,
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
        AccountId: 0,
        SetOrdersDateTime:"",
        OrderStatus: "",
        SubCost: 0,
        Comment:""
    },
    methods:
    {
        toastHint() {
            this.toastHintStyle.fadeInUp = true;
            setTimeout(() => {
                this.toastHintStyle.fadeInUp = false
            },2000)
        },
        watchComment(re) {
            this.recomment = re;
            this.CreateOrEditOrDelete = 'Response';
            this.popupShowing.showPopup = true;
        },
        openmydetails(rso) {
            this.recyleSubscribeOrders = rso;
            this.CreateOrEditOrDelete = 'Details';
            this.popupShowing.showPopup = true;
        },
        createComment(id) {
            this.ordersId = id;
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
            axios.get(`https://localhost:7086/MyOrders/GetMyOrders/${id}`).then(response => {
                this.od = response.data;
                this.mycomment = response.data.comment;
            })
        },
        CreateComment(od) {
            let _this = this;
            var request = null;
            createCommentList = {
                reviewId: _this.createCommentList.reviewId,
                ratingStar: _this.createCommentList.ratingStar,
                comment: _this.createCommentList.comment,
                ordersId: _this.ordersId
            }
            
            axios.post('https://localhost:7086/MyOrders/CreateComment/', createCommentList).then(response => {
                this.toast = response.data;
                this.closepopupShowHint();
                this.GetDetail();
            })
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
        this.GetDetail(id);
    }
})